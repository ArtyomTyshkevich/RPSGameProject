import { HttpHandlerFn, HttpInterceptorFn, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../../services/auth-service.service';
import { catchError, switchMap, throwError } from 'rxjs';

export const authTokenInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token: string | null = authService.token;

  if (!token) {
    return next(req);
  }
  const clonedRequest = req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`
    }
  });

  return next(clonedRequest).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        return refreshAndProceed(authService, req, next);
      }
      return throwError(() => error);
    })
  );
};

const refreshAndProceed = (authService: AuthService, req: HttpRequest<any>, next: HttpHandlerFn) => {
  return authService.refreshTokens().pipe(
    switchMap((newToken) => {
      const newRequest = req.clone({
        setHeaders: {
          Authorization: `Bearer ${newToken.accessToken}`
        }
      });
      return next(newRequest);
    }),
    catchError(error => {
      console.error('Failed to refresh token:', error);
      authService.logout();
      return throwError(() => error);
    })
  );
};
