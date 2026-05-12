import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { TourService } from '../services/tour.service';
import { FollowService } from '../services/follow.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  isLoggedIn = false;
  isAdmin = false;
  featuredTours: any[] = [];
  recommendedProfiles: any[] = [];

  constructor(
    private authService: AuthService, 
    private tourService: TourService, 
    private followService: FollowService) {}

  ngOnInit(): void {
    this.authService.isLoggedIn().subscribe(status => {
      this.isLoggedIn = status;
      this.loadFeaturedTours();
      this.loadRecommendedProfiles();
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
        const filtered = data
          .filter((t: any) => t.status === 1)
          .slice(0, 3);
        this.featuredTours = filtered;
        this.featuredTours.forEach(tour => {
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
      },
      error: (err) => {
        console.log("Error loading tours:", err);
      }
    });
  }

  getDifficultyLabel(difficulty: any): string {
    if (difficulty === 0 || difficulty === 'EASY') return 'EASY';
    if (difficulty === 1 || difficulty === 'MEDIUM') return 'MEDIUM';
    if (difficulty === 2 || difficulty === 'HARD') return 'HARD';
    return 'UNKNOWN';
  }

  loadRecommendedProfiles(): void {
    this.followService.getRecommendations().subscribe({
      next: (profiles) => {
        this.recommendedProfiles = profiles;
      },
      error: (err: any) => {
        console.error(err)
      }
    });
  }

  followUser(userId: number): void {
    this.followService.follow(userId).subscribe({
      next: () => {
        this.recommendedProfiles =
          this.recommendedProfiles.filter(user => user.id !== userId);
      },
      error: (err) => console.error(err)
    });
  }

  getInitials(username: string): string {
    return username ? username.substring(0, 2).toUpperCase() : 'AU';
  }
}