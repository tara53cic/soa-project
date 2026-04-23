import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TourService } from '../services/tour.service';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-my-tours',
  templateUrl: './my-tours.component.html',
  styleUrls: ['./my-tours.component.css']
})
export class MyToursComponent implements OnInit {
  tours: any[] = []; 
  currentUser: any;

  constructor(private tourService: TourService, private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.authService.getCurrentUser().subscribe(user => {
      this.currentUser = user;

      if(this.currentUser) {
        this.loadTours();
      }
    });
  }

  loadTours(): void {
    this.tourService.getToursByAuthor(this.currentUser.id).subscribe({
      next: (data) => {
        this.tours = data;
      },
      error: (err) => {
        console.log("Error while loading tours:", err);
      }
    })
  }

  onCreateTour(): void {
    this.router.navigate(['/create-tour']);
  }

  onViewTour(tour: any): void {
    if (tour.status === 0) {
      this.router.navigate(['/tour-details', tour.id]);
    }
    else {
      this.router.navigate(['/tour-page', tour.id]);
    }
  }

  getStatusLabel(status: any): string {
  if (status === 0 || status === 'DRAFT') return 'DRAFT';
  if (status === 1 || status === 'PUBLISHED') return 'PUBLISHED';
  if (status === 2 || status === 'ARCHIVED') return 'ARCHIVED';
  return 'UNKNOWN';
}

  getStatusClass(status: any): string {
    if (status === 0 || status === 'DRAFT') return 'status-draft';
    if (status === 1 || status === 'PUBLISHED') return 'status-published';
    if (status === 2 || status === 'ARCHIVED') return 'status-archived';
    return '';
  }

  getDifficultyLabel(difficulty: any): string {
    if (difficulty === 0 || difficulty === 'EASY') return 'EASY';
    if (difficulty === 1 || difficulty === 'MEDIUM') return 'MEDIUM';
    if (difficulty === 2 || difficulty === 'HARD') return 'HARD';
    return 'UNKNOWN';
  }

  getDifficultyClass(difficulty: any): string {
    if (difficulty === 0 || difficulty === 'EASY') return 'diff-easy';
    if (difficulty === 1 || difficulty === 'MEDIUM') return 'diff-medium';
    if (difficulty === 2 || difficulty === 'HARD') return 'diff-hard';
    return '';
  }
}