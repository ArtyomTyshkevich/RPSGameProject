import { Routes } from '@angular/router';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { RatingPageComponent } from './pages/rating-page/rating-page.component';
import { canActivateAuth } from './cores/guards/access.guard';
import { RegisterPageComponent } from './pages/register-page/register-page.component';

export const routes: Routes = [
  { path: '', component: RatingPageComponent, canActivate: [canActivateAuth] },
  { path: 'login', component: LoginPageComponent },
  { path: "register", component: RegisterPageComponent}
];
