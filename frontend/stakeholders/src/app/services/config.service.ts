import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  private _api_url = 'http://localhost:8080/auth';
  private _base_url= 'http://localhost:8080/api';
  private _tour_base_url= 'https://localhost:44345/api';

  get login_url() { return `${this._api_url}/login`; }
  get register_url() { return `${this._api_url}/register`; }
  get whoami_url() { return `${this._base_url}/users/whoami`; }
  get logout_url() { return `${this._api_url}/logout`; }
  get users_url() { return `${this._base_url}/users`; }
  get tours_url() { return `${this._tour_base_url}/tours`; }
}