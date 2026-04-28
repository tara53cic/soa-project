import { Component, OnInit, AfterViewInit } from '@angular/core';
import { TourService } from '../services/tour.service';
import { AuthService } from '../services/auth.service';
import { TouristPosition } from '../models/tourist-position.model';
import * as L from 'leaflet';

const myIcon = L.icon({
  iconUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-icon.png',
  iconRetinaUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-icon-2x.png',
  shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-shadow.png',
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -30]
});

@Component({
  selector: 'app-position-simulator',
  templateUrl: './position-simulator.component.html',
  styleUrls: ['./position-simulator.component.css']
})
export class PositionSimulatorComponent implements OnInit, AfterViewInit {
  map!: L.Map;
  touristMarker: L.Marker | null = null;
  currentPosition: TouristPosition | null = null;
  touristId: number = 0;

  constructor(private tourService: TourService, private authService: AuthService) {}

  ngOnInit(): void {
    this.authService.getCurrentUser().subscribe(user => {
      if (user) {
        this.touristId = user.id;
        this.loadCurrentPosition();
      }
    });
  }

  ngAfterViewInit(): void {
    this.initMap();
  }

  private initMap(): void {
    this.map = L.map('map').setView([45.2671, 19.8335], 13);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png').addTo(this.map);
    
    setTimeout(() => this.map.invalidateSize(), 200);

    this.map.on('click', (e: L.LeafletMouseEvent) => {
      this.saveNewPosition(e.latlng.lat, e.latlng.lng);
    });
  }

  loadCurrentPosition(): void {
    this.tourService.getTouristPosition(this.touristId).subscribe({
      next: (data) => {
        this.currentPosition = data;
        if (data && data.latitude && data.longitude) {
          this.drawMarker(data.latitude, data.longitude);
          this.map.setView([data.latitude, data.longitude], 15);
        }
      },
      error: () => console.log("No previous position found for this tourist.")
    });
  }

  saveNewPosition(lat: number, lng: number): void {
    const positionData: TouristPosition = {
      touristId: this.touristId,
      latitude: lat,
      longitude: lng
    };

    this.tourService.updateTouristPosition(positionData).subscribe({
      next: (updatedPos) => {
        this.currentPosition = updatedPos;
        this.drawMarker(lat, lng);
      },
      error: (err) => console.error("Sync error:", err)
    });
  }

  private drawMarker(lat: number, lng: number): void {
    if (this.touristMarker) {
      this.map.removeLayer(this.touristMarker);
    }
    const popupOptions = {
      className: 'custom-rect-popup',
      closeButton: false,
    };
    this.touristMarker = L.marker([lat, lng], { icon: myIcon })
      .addTo(this.map)
      .bindPopup("Your Current Location", popupOptions)
      .openPopup();
    }
}