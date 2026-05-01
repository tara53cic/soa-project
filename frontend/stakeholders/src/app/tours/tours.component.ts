import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TourService } from '../services/tour.service';

@Component({
  selector: 'app-tours',
  templateUrl: './tours.component.html',
  styleUrls: ['./tours.component.css']
})
export class ToursComponent {
  tours: any[] = [];

  constructor(private tourService: TourService, private router: Router) {}

  ngOnInit(): void {
    this.loadPublishedTours();
  }

  loadPublishedTours(): void {
    this.tourService.getTours().subscribe({
      next: (data) => {
        const published = data.filter((t: any) => t.status === 1);
        this.tours = published;
        this.tours.forEach(tour => {
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
      error: (err) => console.error("Error loading tours:", err)
    });
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
