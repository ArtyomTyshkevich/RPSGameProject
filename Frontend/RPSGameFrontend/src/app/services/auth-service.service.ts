import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { AuthRequest } from '../cores/models/auth/AuthRequest';
import { AuthResponse } from '../cores/models/auth/AuthResponse';
import { Observable, tap } from 'rxjs'; 
import { TokenModel } from '../cores/models/auth/TokenModel';
import { RegisterRequest } from '../cores/models/auth/RegisterRequest';
import { jwtDecode } from 'jwt-decode';
import { environment } from '../enviroment/environment';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private cookieService = inject(CookieService);
  router = inject(Router);

  token: string | null = null;
  refreshToken: string | null = null;
  
  get isAuth() {
    this.token = this.token || this.cookieService.get('token');
    return !!this.token;
  }

  login(authRequest: AuthRequest): Observable<AuthResponse> {
    const url = `${environment.authServiceUrl}/auth/login`;
    return this.http.post<AuthResponse>(url, authRequest).pipe(
      tap(val => {
        this.setTokens(val.token, val.refreshToken);
      })
    );
  }

  register(registerRequest: RegisterRequest): Observable<AuthResponse> {
    const url = `${environment.authServiceUrl}/auth/register`;
    return this.http.post<AuthResponse>(url, registerRequest).pipe(
      tap(val => {
        this.setTokens(val.token, val.refreshToken);
      })
    );
  }

  refreshTokens(): Observable<TokenModel> {
    const url = `${environment.authServiceUrl}/auth/refresh-token`;

    if (!this.refreshToken) {
      this.refreshToken = this.cookieService.get('refreshToken');
    }
    if (!this.token) {
      this.token = this.cookieService.get('token');
    }

    const tokenModel: TokenModel = {
      accessToken: this.token,
      refreshToken: this.refreshToken
    };

    return this.http.post<TokenModel>(url, tokenModel).pipe(
      tap(val => {
        this.setTokens(val.accessToken, val.refreshToken);
      })
    );
  }

  getUserIdFromToken(): string {
    const token = this.token || this.cookieService.get('token');
    if (!token) {
        console.error("No token available");
        return '';
    }

    try {
        const decodedToken: any = jwtDecode(token);
        return decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"] || '';
    } catch (error) {
        console.error('Token decoding error', error);
        return '';
    }
  }

  getUserRoleFromToken(): string {
    const token = this.token || this.cookieService.get('token');
    if (!token) {
        console.error("No token available");
        return '';
    }
  
    try {
        const decodedToken: any = jwtDecode(token);
        return decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || '';
    } catch (error) {
        console.error('Token decoding error', error);
        return '';
    }
  }
  
  logout(): void {
    this.token = null;
    this.refreshToken = null;
    this.cookieService.delete('token');
    this.cookieService.delete('refreshToken');
    this.router.navigate(["/start"]);
    
  }
  
  private setTokens(token: string, refreshToken: string) {
    this.token = token;
    this.refreshToken = refreshToken;

    this.cookieService.set('token', token);
    this.cookieService.set('refreshToken', refreshToken);
  }
}