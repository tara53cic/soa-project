import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TourService } from '../services/tour.service';
import * as L from 'leaflet';

@Component({
  selector: 'app-tour-details',
  templateUrl: './tour-details.component.html',
  styleUrls: ['./tour-details.component.css']
})
export class TourDetailsComponent implements OnInit, AfterViewInit {
  tourId: number = 0;
  tour: any = {};
  map!: L.Map;
  tempMarker: L.Marker | null = null;
  selectedFile: File | null = null;

  newKeyPoint = { name: '', description: '', image: '', latitude: 0, longitude: 0 };
  newDuration = { timeInMinutes: 0, transportType: 0 };

  constructor(private route: ActivatedRoute, private tourService: TourService, private router: Router) {}

  ngOnInit(): void {
    this.tourId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadTour();
  }

  ngAfterViewInit(): void {
    this.initMap();
  }

  private initMap(): void {
    this.map = L.map('map').setView([45.2671, 19.8335], 13);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png').addTo(this.map);
    
    setTimeout(() => this.map.invalidateSize(), 200);

    this.map.on('click', (e: L.LeafletMouseEvent) => {
      if (this.tempMarker) {
        this.map.removeLayer(this.tempMarker);
      }

      this.newKeyPoint.latitude = e.latlng.lat;
      this.newKeyPoint.longitude = e.latlng.lng;

      this.tempMarker = L.marker([e.latlng.lat, e.latlng.lng]).addTo(this.map)
        .bindPopup(`<b>New Location Selected</b><br>Lat: ${e.latlng.lat.toFixed(4)}`)
        .openPopup();

      this.tempMarker.on('popupclose', () => {
        if (this.tempMarker) {
          this.map.removeLayer(this.tempMarker);
          this.tempMarker = null;
        }
        this.newKeyPoint.latitude = 0;
        this.newKeyPoint.longitude = 0;
      });
    });
  }

  loadTour(): void {
    this.tourService.getTourById(this.tourId).subscribe({
      next: (data) => {
        this.tour = data;
      },
      error: (err) => console.error("Error during tour loading:", err)
    });
  }

  addKeyPoint(): void {
    this.tourService.addKeyPoint(this.tourId, this.newKeyPoint).subscribe(updatedTour => {
      this.tour = updatedTour;
      this.newKeyPoint = { name: '', description: '', image: '', latitude: 0, longitude: 0 };
      if (this.tempMarker) this.map.removeLayer(this.tempMarker);
    });
  }

  addDuration(): void {
    this.tourService.addDuration(this.tourId, this.newDuration).subscribe(updatedTour => {
      this.tour = updatedTour;
    });
  }

  publish(): void {
    this.tourService.publishTour(this.tourId, this.tour.authorId).subscribe({
      next: () => {
        this.router.navigate(['/my-tours']);
      },
      error: (err) => {
        console.log("Error publishing tour:", err);
      }
    });
  }

  canPublish(): boolean {
    return (
      this.tour.name && 
      this.tour.keyPoints?.length >= 2 && 
      this.tour.durations?.length > 0
    );
  }

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }
}
