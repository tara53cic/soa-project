import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ConfigService } from './config.service';

@Injectable({
  providedIn: 'root'
})
export class TourService {

    constructor(private http: HttpClient, private config: ConfigService) {}

    private getHeaders(): HttpHeaders {
        const token = localStorage.getItem('jwt');
        return new HttpHeaders({ Authorization: `Bearer ${token}` });
    }

    getToursByAuthor(authorId: number): Observable<any[]> {
        return this.http.get<any[]>(`${this.config.tours_url}/author/${authorId}`, { 
            headers: this.getHeaders() 
        });
    }

    createTour(tourData: any): Observable<any> {
        return this.http.post<any>(this.config.tours_url, tourData, {
            headers: this.getHeaders()
        })
    }

    publishTour(tourId: number, authorId: number): Observable<any> {
        return this.http.put<any>(`${this.config.tours_url}/${tourId}/publish`, authorId, { 
            headers: this.getHeaders() 
        });
    }

    addKeyPoint(tourId: number, formData: any): Observable<any> {
        return this.http.post<any>(`${this.config.tours_url}/${tourId}/keypoint`, formData, {
            headers: this.getHeaders()
        });
    }

    addDuration(tourId: number, newDuration: any): Observable<any> {
        return this.http.post<any>(`${this.config.tours_url}/${tourId}/duration`, newDuration, {
            headers: this.getHeaders()
        });
    }

    getTourById(id: number): Observable<any> {
        return this.http.get<any>(`${this.config.tours_url}/${id}`, {
            headers: this.getHeaders()
        });
    }
}