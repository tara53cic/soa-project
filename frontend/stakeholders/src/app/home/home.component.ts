import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  isLoggedIn = false;
  isAdmin = false;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.authService.isLoggedIn().subscribe(status => {
      this.isLoggedIn = status;
      const token = localStorage.getItem('jwt');

      if (status && token) {
        this.authService.getCurrentUser().subscribe({
          next: (res: any) => {
            this.isAdmin = res?.role === 'ROLE_ADMIN' || res?.role?.name === 'ROLE_ADMIN';
          },
          error: () => {
            this.isAdmin = false;
          }
        });
      } else {
        this.isAdmin = false;
      }
    });
  }
}