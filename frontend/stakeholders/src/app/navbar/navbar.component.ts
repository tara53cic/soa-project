import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { PurchaseService } from '../services/purchase.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit { 
  user: any;
  isLoggedIn = false;
  cartItemCount = 0;

  constructor(
    private authService: AuthService, 
    private router: Router,
    private purchaseService: PurchaseService
  ) {}

  ngOnInit(): void {
    this.authService.isLoggedIn().subscribe(
      loggedIn => this.isLoggedIn = loggedIn
    );
    this.authService.getCurrentUser().subscribe({
      next: (data) => {
        this.user = data;
        if (this.user && this.user.role?.name === 'ROLE_TOURIST') {
          this.purchaseService.getCart(this.user.id).subscribe();
          this.purchaseService.cartItemCount$.subscribe(count => {
            this.cartItemCount = count;
          });
        }
      },
      error: (err) => {
        console.log("Error loading user.", err);
      }
    });
  }

  logout(): void {
    this.authService.logout().subscribe({
      next: () => {
        this.isLoggedIn = false;
        this.router.navigate(['/login']);
      },
      error: (err) => {
        localStorage.clear();
        this.isLoggedIn = false;
        this.router.navigate(['/login']);
      }
    });
  }

  goToLogin(): void {
    this.router.navigate(['/login']);
  }

  goToSignUp(): void {
    this.router.navigate(['/register']);
  }

  goToProfile(): void {
    this.router.navigate(['/profile']);
  }
}
