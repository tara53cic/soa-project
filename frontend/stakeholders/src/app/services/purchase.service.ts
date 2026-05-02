import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { ConfigService } from './config.service';
import { ShoppingCart } from '../models/shopping-cart.model';
import { OrderItem } from '../models/order-item.model';

@Injectable({
  providedIn: 'root'
})
export class PurchaseService {

  private cartItemCountSubject = new BehaviorSubject<number>(0);
  public cartItemCount$ = this.cartItemCountSubject.asObservable();

  constructor(private http: HttpClient, private configService: ConfigService) { }

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('jwt');
    return new HttpHeaders({ Authorization: `Bearer ${token}` });
  }

  getCart(touristId: number): Observable<ShoppingCart> {
    return this.http.get<ShoppingCart>(`${this.configService.shopping_cart_url}/${touristId}`, { headers: this.getHeaders() })
      .pipe(
        tap(cart => this.cartItemCountSubject.next(cart?.items?.length || 0))
      );
  }

  addToCart(touristId: number, itemDto: OrderItem): Observable<ShoppingCart> {
    return this.http.post<ShoppingCart>(`${this.configService.shopping_cart_url}/${touristId}/add`, itemDto, { headers: this.getHeaders() })
      .pipe(
        tap(cart => this.cartItemCountSubject.next(cart?.items?.length || 0))
      );
  }

  removeFromCart(touristId: number, tourId: number): Observable<ShoppingCart> {
    return this.http.delete<ShoppingCart>(`${this.configService.shopping_cart_url}/${touristId}/remove/${tourId}`, { headers: this.getHeaders() })
      .pipe(
        tap(cart => this.cartItemCountSubject.next(cart?.items?.length || 0))
      );
  }

  checkout(touristId: number): Observable<any[]> {
    return this.http.post<any[]>(`${this.configService.shopping_cart_url}/${touristId}/checkout`, {}, { headers: this.getHeaders() })
      .pipe(
        tap(() => this.cartItemCountSubject.next(0))
      );
  }

  hasPurchasedTour(touristId: number, tourId: number): Observable<boolean> {
    return this.http.get<boolean>(`${this.configService.shopping_cart_url}/${touristId}/has-purchased/${tourId}`, { headers: this.getHeaders() });
  }
}

