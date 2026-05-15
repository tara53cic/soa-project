import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { HttpParams } from "@angular/common/http";

@Injectable({
    providedIn: 'root'
})
export class FollowService {
    private apiUrl = 'http://localhost:8083';

    constructor(private http: HttpClient) {}

    follow(followingUsername: string) {
        const followerUsername = this.getUsernameFromToken();
        const token = localStorage.getItem('jwt');

        return this.http.post(
        `${this.apiUrl}/follow`,
        {
            followerUsername: followerUsername,
            followingUsername: followingUsername
        },
        {
            headers: {
            Authorization: `Bearer ${token}`
            },
            responseType: 'text'
        }
        );
    }

    isFollowing(followingUsername: string) {
        const followerUsername = this.getUsernameFromToken();
        const token = localStorage.getItem('jwt');

        const params = new HttpParams()
        .set('followerUsername', followerUsername || '')
        .set('followingUsername', followingUsername);

        return this.http.get<{ isFollowing: boolean }>(
        `${this.apiUrl}/is-following`,
        {
            params,
            headers: {
            Authorization: `Bearer ${token}`
            }
        }
        );
    }

    getRecommendations() {
        const token = localStorage.getItem('jwt');

        return this.http.get<any[]>(
            'http://localhost:8083/recommendations',
            {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            }
        );
    }

    private getUsernameFromToken(): string | null {
        const token = localStorage.getItem('jwt');

        if (!token) return null;

        const payload = JSON.parse(atob(token.split('.')[1]));
        return payload.sub;
    }
}