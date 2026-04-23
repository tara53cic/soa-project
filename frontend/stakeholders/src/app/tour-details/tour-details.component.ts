import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TourService } from '../services/tour.service';
import * as L from 'leaflet';
import 'leaflet-routing-machine';

const myIcon = L.icon({
  iconUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-icon.png',
  iconRetinaUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-icon-2x.png',
  shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-shadow.png',
  iconSize: [25, 41],
  iconAnchor: [12, 41]
});

@Component({
  selector: 'app-tour-details',
  templateUrl: './tour-details.component.html',
  styleUrls: ['./tour-details.component.css']
})
export class TourDetailsComponent implements OnInit, AfterViewInit {
  private routingControl: any;
  tourId: number = 0;
  tour: any = {};
  map!: L.Map;
  tempMarker: L.Marker | null = null;
  selectedFile: File | null = null;
  private markers: L.Marker[] = [];

  editingKeyPoint: any = null;
  isEditing: boolean = false;

  newKeyPoint = { name: '', description: '', image: '', latitude: 0, longitude: 0 };
  newDuration = { minutes: 0, travelType: 0 };

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

      if (this.isEditing) {
        this.editingKeyPoint.latitude = e.latlng.lat;
        this.editingKeyPoint.longitude = e.latlng.lng;
      } else {
        this.newKeyPoint.latitude = e.latlng.lat;
        this.newKeyPoint.longitude = e.latlng.lng;
      }

      this.tempMarker = L.marker([e.latlng.lat, e.latlng.lng], { icon: myIcon })
        .addTo(this.map)
        .bindPopup("Location selected")
        .openPopup();

        this.tempMarker.on('popupclose', () => {
          if (this.tempMarker) {
            this.map.removeLayer(this.tempMarker);
            this.tempMarker = null;
            
            if (this.isEditing) {
              this.editingKeyPoint.latitude = 0;
              this.editingKeyPoint.longitude = 0;
            } else {
              this.newKeyPoint.latitude = 0;
              this.newKeyPoint.longitude = 0;
            }
          }
        });
    });
  }

  loadTour(): void {
    this.tourService.getTourById(this.tourId).subscribe({
      next: (data) => {
        this.tour = data;
        this.renderKeyPoints();
      },
      error: (err) => console.error("Error during tour loading:", err)
    });
  }

  addKeyPoint() {
    const formData = new FormData();
    
    formData.append('Name', this.newKeyPoint.name);
    formData.append('Description', this.newKeyPoint.description);
    formData.append('Longitude', this.newKeyPoint.longitude.toString());
    formData.append('Latitude', this.newKeyPoint.latitude.toString());
    formData.append('TourId', this.tourId.toString());

    if (this.selectedFile) {
      formData.append('Image', this.selectedFile, this.selectedFile.name);
    }

      this.tourService.addKeyPoint(this.tourId, formData).subscribe({
        next: (updatedTour) => {
          console.log('Added keypoint', updatedTour);
          this.loadTour();

          this.newKeyPoint = { name: '', description: '', image: '', latitude: 0, longitude: 0 };
          this.selectedFile = null;
          if (this.tempMarker) {
            this.map.removeLayer(this.tempMarker);
            this.tempMarker = null;
          }
        },
        error: (err) => console.error('Server error:', err)
      });
  }

  addDuration(): void {
    this.tourService.addDuration(this.tourId, this.newDuration).subscribe(updatedTour => {
      this.loadTour();
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
      this.tour.duration?.length > 0
    );
  }

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  private renderKeyPoints(): void {
    this.markers.forEach(m => this.map.removeLayer(m));
    this.markers = [];

    if (this.routingControl) {
      this.map.removeControl(this.routingControl);
      this.routingControl = null;
    }

    if (!this.tour || !this.tour.keyPoints || this.tour.keyPoints.length === 0) {
      return;
    }

    this.tour.keyPoints.forEach((kp: any) => {
      const marker = L.marker([kp.latitude, kp.longitude], { icon: myIcon })
        .addTo(this.map)
        .bindPopup(`
          <div class="popup-custom-content">
            <b>${kp.name}</b>
            <div class="popup-btn-wrapper">
              <button class="btn-edit-popup" id="edit-${kp.id}">Edit</button>
              <button class="btn-delete-popup" id="delete-${kp.id}">Delete</button>
            </div>
          </div>
        `);

      (marker as any).id = kp.id; 
      this.markers.push(marker);

      marker.on('popupopen', () => {
        setTimeout(() => {
          const editBtn = document.getElementById(`edit-${kp.id}`);
          const deleteBtn = document.getElementById(`delete-${kp.id}`);
          if (editBtn) editBtn.onclick = () => this.startEditing(kp);
          if (deleteBtn) deleteBtn.onclick = () => this.deleteKeyPoint(kp.id);
        }, 100);
      });
    });

    if (this.tour.keyPoints.length >= 2) {
      const waypoints = this.tour.keyPoints.map((kp: any) => L.latLng(kp.latitude, kp.longitude));

      this.routingControl = L.Routing.control({
        waypoints: waypoints,
        routeWhileDragging: false,
        addWaypoints: false,
        show: false,
        lineOptions: {
          styles: [{ color: 'blue', opacity: 0.6, weight: 6 }],
          extendToWaypoints: true,
          missingRouteTolerance: 10
        },
        createMarker: () => null 
      } as any).addTo(this.map);
    }
  }

  startEditing(kp: any) {
    this.isEditing = true;
    this.editingKeyPoint = { ...kp };
    setTimeout(() => this.map.invalidateSize(), 200);

    if (this.tempMarker) {
      this.map.removeLayer(this.tempMarker);
    }

    this.tempMarker = L.marker([kp.latitude, kp.longitude], { icon: myIcon })
      .addTo(this.map)
      .bindPopup("Editing location")
      .openPopup();
  }

  savePrice() {
    this.tourService.updatePrice(this.tourId, this.tour.price).subscribe({
      next: (val) => console.log('Price update.', val),
      error: (err) => console.error('Error saving price', err)
    });
  }

  updateKeyPoint() {
    const formData = new FormData();

    formData.append('Id', this.editingKeyPoint.id);
    formData.append('TourId', this.tourId.toString());

    formData.append('Name', this.editingKeyPoint.name);
    formData.append('Description', this.editingKeyPoint.description);
    formData.append('Longitude', this.editingKeyPoint.longitude.toString());
    formData.append('Latitude', this.editingKeyPoint.latitude.toString());

    if (this.selectedFile) {
      formData.append('Image', this.selectedFile, this.selectedFile.name);
    }

    this.tourService.updateKeyPoint(this.editingKeyPoint.id, formData).subscribe({
      next: () => {
        this.isEditing = false;
        this.editingKeyPoint = null;
        this.selectedFile = null;

        if (this.tempMarker) {
          this.map.removeLayer(this.tempMarker);
          this.tempMarker = null;
        }

        this.loadTour();
      },
      error: (err) => console.error('Update error', err)
    });
  }

  cancelEditing() {
    this.isEditing = false;
    this.editingKeyPoint = null;
    this.selectedFile = null;
    setTimeout(() => this.map.invalidateSize(), 200);

    if (this.tempMarker) {
      this.map.removeLayer(this.tempMarker);
      this.tempMarker = null;
    }
  }

  deleteKeyPoint(id: number) {
    if (!confirm('Are you sure you want to delete this key point?')) return;

    this.tourService.deleteKeyPoint(id).subscribe({
      next: () => {
        const markerIndex = this.markers.findIndex(m => (m as any).id === id);
        if (markerIndex !== -1) {
          this.map.removeLayer(this.markers[markerIndex]);
          this.markers.splice(markerIndex, 1);
        }

        this.loadTour();
      },
      error: (err) => console.error('Delete error', err)
    });
  }
}
