import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { TourService } from '../services/tour.service';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-create-tour',
  templateUrl: './create-tour.component.html',
  styleUrls: ['./create-tour.component.css']
})
export class CreateTourComponent {

  tour = {
    name: '',
    description: '',
    difficulty: 0,
    tags: '',
    authorId: 0
  }

  constructor(private tourService: TourService, private authService: AuthService, private router: Router) {
    this.authService.getCurrentUser().subscribe(user => {
      if (user) this.tour.authorId = user.id;
    });
  }

  create(shouldRedirectToDetails: boolean): void {
    if (!this.tour.name || !this.tour.description || !this.tour.tags) {
      alert("All fields should be filled before saving.");
      return;
    }

    const tourData = {
      ...this.tour,
      tags: this.tour.tags.split(',').map(t => t.trim())
    };

    this.tourService.createTour(tourData).subscribe({
      next: (createdTour) => {
        if (shouldRedirectToDetails) {
          this.router.navigate(['/tour-details']);          
        } else {
          this.router.navigate(['/my-tours']);
        }
      },
      error: (err) => {
        console.log("Error during tour creation:", err);
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/my-tours']);
  }

}
