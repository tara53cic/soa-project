import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  isLoggedIn = false;
  isAdmin = false;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.authService.isLoggedIn().subscribe(status => {
      this.isLoggedIn = status;
      if (status) {
        this.authService.getCurrentUser().subscribe({
          next: (res: any) => {
            this.isAdmin = !!(res && res.role && res.role.name === 'ROLE_ADMIN');
          },
          error: () => { this.isAdmin = false; }
        });
      } else {
        this.isAdmin = false;
      }
    });
  }
}
