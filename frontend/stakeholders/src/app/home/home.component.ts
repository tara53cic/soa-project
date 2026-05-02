import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { TourService } from '../services/tour.service';
import { PurchaseService } from '../services/purchase.service';
import { forkJoin, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  isLoggedIn = false;
  isAdmin = false;
  featuredTours: any[] = [];
  user: any;

  constructor(
     private authService: AuthService, 
     private tourService: TourService,
     private purchaseService: PurchaseService
  ) {}

  ngOnInit(): void {
    this.authService.isLoggedIn().subscribe(status => {
      this.isLoggedIn = status;
      const token = localStorage.getItem('jwt');

      if (status && token) {
        this.authService.getCurrentUser().subscribe({
          next: (res: any) => {
            this.user = res;
            this.isAdmin = res?.role === 'ROLE_ADMIN' || res?.role?.name === 'ROLE_ADMIN';
            this.loadFeaturedTours();
          },
          error: () => {
            this.isAdmin = false;
            this.loadFeaturedTours();
          }
        });
      } else {
        this.isAdmin = false;
        this.loadFeaturedTours();
      }
    });
  }

  loadFeaturedTours(): void {
    this.tourService.getTours().subscribe({
      next: (data) => {
        const published = data.filter((t: any) => t.status === 1).slice(0, 5); // Just some featured logic

        published.forEach(tour => {
          this.tourService.getReviewsByTour(tour.id).subscribe({
            next: (reviews) => {
              tour.reviews = reviews;
              if (reviews && reviews.length > 0) {
                const sum = reviews.reduce((acc: number, r: any) => acc + r.grade, 0);
                tour.averageRating = sum / reviews.length;
              } else {
                tour.averageRating = 0;
              }
            }
          });
        });

        if (this.user && this.user.role?.name === 'ROLE_TOURIST') {
          const checks = published.map((tour: any) => 
            this.purchaseService.hasPurchasedTour(this.user.id, tour.id).pipe(
              catchError(() => of(false))
            )
          );

          if (checks.length > 0) {
            forkJoin(checks).subscribe((results: any[]) => {
              this.featuredTours = [];
              results.forEach((hasPurchased, index) => {
                // only add not purchased for featured
                if (!hasPurchased) {
                  this.featuredTours.push(published[index]);
                }
              });
            });
          }
        } else {
          this.featuredTours = published;
        }
      },
      error: (err) => console.error("Error loading featured tours:", err)
    });
  }

  getDifficultyLabel(difficulty: any): string {
    if (difficulty === 0 || difficulty === 'EASY') return 'EASY';
    if (difficulty === 1 || difficulty === 'MEDIUM') return 'MEDIUM';
    if (difficulty === 2 || difficulty === 'HARD') return 'HARD';
    return 'UNKNOWN';
  }
}
