import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Injectable({
    providedIn: 'root'
})
export class FollowService {
    private apiUrl = 'http://localhost:8083';

    constructor(private http: HttpClient) {}

    follow(userId: number) {
        return this.http.post(`${this.apiUrl}/${userId}`, {});
    }

    isFollowing(userId: number) {
        return this.http.get<boolean>(`${this.apiUrl}/${userId}/is-following`);
    }

    getRecommendations() {
        const token = localStorage.getItem('jwt');

        console.log('JWT TOKEN:', token);

        return this.http.get<any[]>(
            'http://localhost:8083/recommendations',
            {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            }
        );
    }
}