import { Routes } from '@angular/router';
import {LoginPageComponent} from './pages/login-page/login-page.component'
import { RatingPageComponent } from './pages/rating-page/rating-page.component';

export const routes: Routes = [
    {path: '', component: RatingPageComponent},
    {path: 'login', component: LoginPageComponent}
];
