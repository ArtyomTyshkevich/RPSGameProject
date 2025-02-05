import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { AuthRequest } from '../cores/models/auth/AuthRequest';
import { AuthResponse } from '../cores/models/auth/AuthResponse';
import { Observable, tap } from 'rxjs'; 
import { TokenModel } from '../cores/models/auth/TokenModel';
import { RegisterRequest } from '../cores/models/auth/RegisterRequest';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private cookieService = inject(CookieService);

  token: string | null = null;
  refreshToken: string | null = null;
  
  get isAuth() {
    this.token = this.token || this.cookieService.get('token');
    return !!this.token;
  }

  login(authRequest: AuthRequest): Observable<AuthResponse> {
    const url = 'https://localhost/auth/login';
    return this.http.post<AuthResponse>(url, authRequest).pipe(
      tap(val => {
        this.setTokens(val.token, val.refreshToken);
      })
    );
  }
  register(registerRequest: RegisterRequest): Observable<AuthResponse> {
    const url = 'https://localhost/auth/register';
    return this.http.post<AuthResponse>(url, registerRequest).pipe(
      tap(val => {
        this.setTokens(val.token, val.refreshToken);
      })
    );
  }

  refreshTokens(): Observable<TokenModel> {
    const url = 'https://localhost/auth/refresh-token';

    if (!this.refreshToken) {
      this.refreshToken = this.cookieService.get('refreshToken');
    }

    if (!this.refreshToken) {
      throw new Error("No refresh token available");
    }

    const tokenModel: TokenModel = {
      token: this.token || '',
      refreshToken: this.refreshToken
    };
    console.log(tokenModel)
    return this.http.post<TokenModel>(url, tokenModel).pipe(
      tap(val => {
        this.setTokens(val.token, val.refreshToken);
      })
    );
  }

  private setTokens(token: string, refreshToken: string) {
    this.token = token;
    this.refreshToken = refreshToken;

    this.cookieService.set('token', token);
    this.cookieService.set('refreshToken', refreshToken);
  }
}
