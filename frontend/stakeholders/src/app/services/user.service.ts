import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ConfigService } from './config.service';
import { User } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class UserService {
  constructor(private http: HttpClient, private config: ConfigService) {}

  private headers(): HttpHeaders {
    const token = localStorage.getItem('jwt');
    return new HttpHeaders({ Authorization: `Bearer ${token}` });
  }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.config.users_url, { headers: this.headers() });
  }

  getUserById(id: number): Observable<User> {
    const url = `${this.config.users_url}/${id}`;
    return this.http.get<User>(url, { headers: this.headers() });
  }

  blockUser(id: number, block: boolean) {
    const url = `${this.config.users_url}/${id}/block?block=${block}`;
    return this.http.put(url, {}, { headers: this.headers(), responseType: 'text' });
  }

  updateProfile(userData: any): Observable<any> {
    return this.http.put(`${this.config.users_url}/profile`, userData, { headers: this.headers() });
  }
}
