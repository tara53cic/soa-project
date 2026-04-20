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
    })
    if(this.currentUser) {
      this.loadTours();
    }
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

  onViewTour(id: number): void {
    this.router.navigate(['/tour-details', id]);
  }
}