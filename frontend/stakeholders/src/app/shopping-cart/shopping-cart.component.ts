import { Component, OnInit } from '@angular/core';
import { PurchaseService } from '../services/purchase.service';
import { TourService } from '../services/tour.service';
import { AuthService } from '../services/auth.service';
import { OrderItem } from '../models/order-item.model';

@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.css']
})
export class ShoppingCartComponent implements OnInit {
  orderItems: any[] = [];
  totalPrice: number = 0;
  touristId!: number;

  constructor(
    private purchaseService: PurchaseService,
    private tourService: TourService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.authService.getCurrentUser().subscribe({
      next: (user: any) => {
        if (user) {
          this.touristId = user.id;
          this.loadCart();
        }
      }
    });
  }

  loadCart() {
    this.purchaseService.getCart(this.touristId).subscribe({
      next: (cart) => {
        this.totalPrice = cart.totalPrice;
        this.orderItems = [];
        
        cart.items.forEach((item: OrderItem) => {
          this.tourService.getTourById(item.tourId).subscribe((tourData: any) => {
            this.orderItems.push({
              tour: tourData,
              price: item.price
            });
          });
        });
      },
      error: () => {
        this.orderItems = [];
        this.totalPrice = 0;
      }
    });
  }

  removeFromCart(tourId: number) {
    this.purchaseService.removeFromCart(this.touristId, tourId).subscribe({
      next: () => {
        this.loadCart();
      }
    });
  }

  checkout() {
    this.purchaseService.checkout(this.touristId).subscribe({
      next: () => {
        alert('Checkout successful! Tours unlocked.');
        this.loadCart();
      },
      error: (err) => {
        alert('Checkout failed: ' + err.message);
      }
    });
  }

  getImageUrl(imagePath: string): string {
    return 'http://localhost:8000/tours/' + imagePath;
  }
}
