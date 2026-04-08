import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ConfigService } from './config.service';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { finalize, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loggedIn = new BehaviorSubject<boolean>(false);

  constructor(
    private http: HttpClient,
    private config: ConfigService,
    private router: Router
  ) {
    const status = localStorage.getItem('isLoggedIn') === 'true';
    this.loggedIn.next(status);
  }

  login(username: string, password: string): Observable<any> {
    const loginData = {
      username: username,
      password: password
    };

  return this.http.post(this.config.login_url, loginData).pipe(
    tap((res: any) => {
      this.loggedIn.next(true);
      localStorage.setItem('isLoggedIn', 'true');
      if (res.accessToken) {
        localStorage.setItem('jwt', res.accessToken);
      }
    })
  );
  }

  register(userData: any): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    return this.http.post(
      this.config.register_url, 
      userData,
      { responseType: 'text' }
    );
  }

  logout(): Observable<any> {
    return this.http.post(
      this.config.logout_url, 
      {}, 
      { responseType: 'text' }
    ).pipe(
      finalize(() => {
        this.loggedIn.next(false);
        localStorage.removeItem('isLoggedIn');
        localStorage.removeItem('jwt');
        this.router.navigate(['/']);
      })
    );
  }

  isLoggedIn(): Observable<boolean> {
    return this.loggedIn.asObservable();
  }

  private checkLoginStatus(): void {
    this.http.get(this.config.whoami_url, { 
      withCredentials: true 
    }).subscribe({
      next: () => {
        this.loggedIn.next(true);
        localStorage.setItem('isLoggedIn', 'true');
      },
      error: () => {
        this.loggedIn.next(false);
        localStorage.removeItem('isLoggedIn');
      }
    });
  }

  getCurrentUser(): Observable<any> {
    return this.http.get(this.config.whoami_url, { 
      withCredentials: true 
    });
  }
}