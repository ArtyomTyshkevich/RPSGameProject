import { inject } from '@angular/core';
import { AuthService } from '../../services/auth-service.service';
import { Router } from '@angular/router';
import { UrlTree } from '@angular/router';

export const canActivateAuth = (): boolean | UrlTree => {
  const authService = inject(AuthService);
  const router = inject(Router);
  if (authService.isAuth) {
    return true;
  }

  return router.createUrlTree(['/start']); 
};
export const wasActivated = (): boolean | UrlTree => {
  const authService = inject(AuthService);
  const router = inject(Router);
  if (!authService.isAuth) {
    return true;
  }

  return router.createUrlTree(['']); 
};
