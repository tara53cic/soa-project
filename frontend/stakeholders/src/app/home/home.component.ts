import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { TourService } from '../services/tour.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  isLoggedIn = false;
  isAdmin = false;
  featuredTours: any[] = [];

  constructor(private authService: AuthService, private tourService: TourService) {}

  ngOnInit(): void {
    this.authService.isLoggedIn().subscribe(status => {
      this.isLoggedIn = status;
      this.loadFeaturedTours();
      const token = localStorage.getItem('jwt');

      if (status && token) {
        this.authService.getCurrentUser().subscribe({
          next: (res: any) => {
            this.isAdmin = res?.role === 'ROLE_ADMIN' || res?.role?.name === 'ROLE_ADMIN';
          },
          error: () => {
            this.isAdmin = false;
          }
        });
      } else {
        this.isAdmin = false;
      }
    });
  }

  loadFeaturedTours(): void {
    this.tourService.getTours().subscribe({
      next: (data) => {
        this.featuredTours = data
          .filter((t: any) => t.status === 1)
          .slice(0, 3);
      },
      error: (err) => {
        console.log("Error loading tours:", err);
      }
    })
  }

  getDifficultyLabel(difficulty: any): string {
    if (difficulty === 0 || difficulty === 'EASY') return 'EASY';
    if (difficulty === 1 || difficulty === 'MEDIUM') return 'MEDIUM';
    if (difficulty === 2 || difficulty === 'HARD') return 'HARD';
    return 'UNKNOWN';
  }
}