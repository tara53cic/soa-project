import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TourService } from '../services/tour.service';
import { AuthService } from '../services/auth.service';
import { UserService } from '../services/user.service';
import * as L from 'leaflet';

@Component({
  selector: 'app-tour-page',
  templateUrl: './tour-page.component.html',
  styleUrls: ['./tour-page.component.css']
})
export class TourPageComponent implements OnInit {
  tourId: number = 0;
  tour: any;
  user: any;
  map!: L.Map; 
  usernames: { [key: number]: string } = {};

  reviews: any[] = [];
  selectedFiles: File[] = [];
  newReview = {
    grade: 5,
    comment: '',
    attendanceDate: ''
  };

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private tourService: TourService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.tourId = Number(this.route.snapshot.paramMap.get('id'));
    
    this.authService.getCurrentUser().subscribe(data => {
      this.user = data;
      this.loadTour();
      this.loadReviews();
    });
  }

  loadTour(): void {
    this.tourService.getTourById(this.tourId).subscribe(data => {
      this.tour = data;
      setTimeout(() => {
        const mapElement = document.getElementById('map');
        if (mapElement) {
          this.initMap();
        } else {
          console.error("Map not loaded.");
        }
      }, 50); 
    });
  }

  private initMap(): void {
    if (this.map) {
      this.map.remove();
    }

    const DefaultIcon = L.icon({
      iconUrl: 'https://unpkg.com/leaflet@1.7.1/dist/images/marker-icon.png',
      shadowUrl: 'https://unpkg.com/leaflet@1.7.1/dist/images/marker-shadow.png',
      iconSize: [25, 41],
      iconAnchor: [12, 41]
    });
    L.Marker.prototype.options.icon = DefaultIcon;

    const firstKP = this.tour.keyPoints[0];
    this.map = L.map('map').setView([firstKP.latitude, firstKP.longitude], 13);
    
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png').addTo(this.map);

    if (this.user?.role?.name === 'ROLE_TOURIST') {
      L.marker([firstKP.latitude, firstKP.longitude]).addTo(this.map)
        .bindPopup('Start of the tour').openPopup();
    } else {
      const waypoints = this.tour.keyPoints.map((kp: any) => L.latLng(kp.latitude, kp.longitude));
      
      (L as any).Routing.control({
        waypoints: waypoints,
        show: false,
        addWaypoints: false,
        createMarker: () => null 
      } as any).addTo(this.map);

      this.tour.keyPoints.forEach((kp: any) => {
        L.marker([kp.latitude, kp.longitude]).addTo(this.map).bindPopup(kp.name);
      });
    }
  }

  onArchive(): void {
    this.tourService.archiveTour(this.tourId).subscribe({
      next: () => this.router.navigate(['/my-tours']),
      error: (err) => console.error(err)
    });
  }

  onUnarchive(): void {
    this.tourService.unarchiveTour(this.tourId).subscribe({
      next: () => this.router.navigate(['/my-tours']),
      error: (err) => console.error(err)
    });
  }

  getDifficultyLabel(difficulty: any): string {
    if (difficulty === 0 || difficulty === 'EASY') return 'EASY';
    if (difficulty === 1 || difficulty === 'MEDIUM') return 'MEDIUM';
    if (difficulty === 2 || difficulty === 'HARD') return 'HARD';
    return 'UNKNOWN';
  }

  loadReviews(): void {
    this.tourService.getReviewsByTour(this.tourId).subscribe(data => {
      this.reviews = data;
    });
  }

  onFileSelected(event: any): void {
    this.selectedFiles = Array.from(event.target.files);
  }

  onSubmitReview(): void {
    const formData = new FormData();

    const dateValue = new Date(this.newReview.attendanceDate);
    const utcDate = dateValue.toISOString();

    formData.append('grade', this.newReview.grade.toString());
    formData.append('comment', this.newReview.comment);
    formData.append('touristId', this.user.id.toString());
    formData.append('tourId', this.tourId.toString());
    formData.append('attendanceDate', utcDate);
    formData.append('touristUsername', this.user.username);

    this.selectedFiles.forEach(file => {
      formData.append('imageFiles', file, file.name);
    });

    this.tourService.createReview(formData).subscribe({
      next: () => {
        this.loadReviews();
        this.resetReviewForm();
      },
      error: (err) => console.error(err)
    });
  }

  getRatingDescription(grade: number): string {
    const descriptions: { [key: number]: string } = {
      1: 'Poor - Not worth it',
      2: 'Fair - Could be better',
      3: 'Good - Enjoyable experience',
      4: 'Very Good - Highly recommended',
      5: 'Excellent - Absolutely amazing!'
    };
    return descriptions[grade] || 'Select your rating';
  }

  private resetReviewForm(): void {
    this.newReview = { grade: 5, comment: '', attendanceDate: '' };
    this.selectedFiles = [];
  }
}