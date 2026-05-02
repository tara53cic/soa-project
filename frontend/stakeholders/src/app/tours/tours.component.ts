import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TourService } from '../services/tour.service';
import { PurchaseService } from '../services/purchase.service';
import { AuthService } from '../services/auth.service';
import { forkJoin, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-tours',
  templateUrl: './tours.component.html',
  styleUrls: ['./tours.component.css']
})
export class ToursComponent implements OnInit {
  availableTours: any[] = [];
  purchasedTours: any[] = [];
  activeTab: 'available' | 'purchased' = 'available';
  user: any;

  constructor(
    private tourService: TourService, 
    private router: Router,
    private purchaseService: PurchaseService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.authService.getCurrentUser().subscribe({
      next: (u) => {
        this.user = u;
        this.loadPublishedTours();
      },
      error: () => this.loadPublishedTours()
    });
  }

  loadPublishedTours(): void {
    this.tourService.getTours().subscribe({
      next: (data) => {
        const published = data.filter((t: any) => t.status === 1);
        
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
              this.availableTours = [];
              this.purchasedTours = [];
              results.forEach((hasPurchased, index) => {
                if (hasPurchased) {
                  this.purchasedTours.push(published[index]);
                } else {
                  this.availableTours.push(published[index]);
                }
              });
            });
          }
        } else {
          this.availableTours = published;
          this.purchasedTours = [];
        }
      },
      error: (err) => console.error("Error loading tours:", err)
    });
  }

  get displayedTours() {
    return this.activeTab === 'available' ? this.availableTours : this.purchasedTours;
  }

  setTab(tab: 'available' | 'purchased') {
    this.activeTab = tab;
  }

  viewTourDetails(tourId: number): void {
    this.router.navigate(['/tour-page', tourId]);
  }

  getDifficultyLabel(difficulty: any): string {
    if (difficulty === 0 || difficulty === 'EASY') return 'EASY';
    if (difficulty === 1 || difficulty === 'MEDIUM') return 'MEDIUM';
    if (difficulty === 2 || difficulty === 'HARD') return 'HARD';
    return 'UNKNOWN';
  }
}
