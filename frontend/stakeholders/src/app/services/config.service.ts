import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  private _api_url = 'http://localhost:8080/auth';

  get login_url() { return `${this._api_url}/login`; }
  get register_url() { return `${this._api_url}/register`; }
  get whoami_url() { return `${this._api_url}/whoami`; }
  get logout_url() { return `${this._api_url}/logout`; }
}