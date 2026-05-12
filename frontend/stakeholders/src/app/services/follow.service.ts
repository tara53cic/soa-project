import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Injectable({
    providedIn: 'root'
})
export class FollowService {
    private apiUrl = 'http://localhost:8083/api/follows';

    constructor(private http: HttpClient) {}

  follow(userId: number) {
    return this.http.post(`${this.apiUrl}/${userId}`, {});
  }

  unfollow(userId: number) {
    return this.http.delete(`${this.apiUrl}/${userId}`);
  }

  isFollowing(userId: number) {
    return this.http.get<boolean>(`${this.apiUrl}/${userId}/is-following`);
  }

  getRecommendations() {
    return this.http.get<any[]>(`${this.apiUrl}/recommendations`);
  }
}