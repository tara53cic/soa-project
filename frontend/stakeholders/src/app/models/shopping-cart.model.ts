import { OrderItem } from './order-item.model';

export interface ShoppingCart {
  id: number;
  touristId: number;
  items: OrderItem[];
  totalPrice: number;
}
