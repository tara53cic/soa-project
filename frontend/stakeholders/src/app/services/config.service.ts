import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  private _api_url = 'http://localhost:8000/stakeholders/auth';
  private _base_url= 'http://localhost:8000/stakeholders/api';
  private _tour_base_url= 'http://localhost:8000/tours/api';
  private _purchase_base_url= 'http://localhost:8000/purchase/api';
  //private _gateway_rpc_url = 'http://localhost:8000/gateway/purchase';
  private _gateway_saga_url = 'http://localhost:8000/api/gateway/checkout-saga';
  private _tourist_position_url = 'http://localhost:8000/api/tourist/position';

  get login_url() { return `${this._api_url}/login`; }
  get register_url() { return `${this._api_url}/register`; }
  get whoami_url() { return `${this._base_url}/users/whoami`; }
  get logout_url() { return `${this._api_url}/logout`; }
  get users_url() { return `${this._base_url}/users`; }
  get tours_url() { return `${this._tour_base_url}/tours`; }
  get shopping_cart_url() { return `${this._purchase_base_url}/shopping-cart`;}
  //get checkout_url() { return `${this._gateway_rpc_url}/checkout`; } 
  checkout_url(touristId: number) { 
    return `${this._gateway_saga_url}/${touristId}`; 
  }
  get tourist_position_url() { return this._tourist_position_url; }
}