import { Routes } from '@angular/router';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { RatingPageComponent } from './pages/rating-page/rating-page.component';
import { canActivateAuth, wasActivated } from './cores/guards/access.guard';
import { RegisterPageComponent } from './pages/register-page/register-page.component';
import { MenuPageComponent } from './pages/menu-page/menu-page.component';
import { StartPageComponent } from './pages/start-page/start-page.component';
import { ChatPageComponent } from './pages/chat-page/chat-page.component';
import { ProfilePageComponent } from './pages/profile-page/profile-page.component';
import { GamePageComponent } from './pages/game-page/game-page.component';
import { RoomControlPageComponent } from './pages/room-control-page/room-control-page.component';

export const routes: Routes = [
  { path: 'start', component: StartPageComponent, canActivate: [wasActivated]},
  { path: '', component: MenuPageComponent, canActivate: [canActivateAuth]},
  { path: 'rating', component: RatingPageComponent, canActivate: [canActivateAuth]},
  { path: 'login', component: LoginPageComponent, canActivate: [wasActivated]},
  { path: "register", component: RegisterPageComponent, canActivate: [wasActivated]},
  { path: 'chat', component: ChatPageComponent, canActivate: [canActivateAuth]},
  { path: 'profile/:userId', component: ProfilePageComponent, canActivate: [canActivateAuth]},
  { path: 'game', component: GamePageComponent, canActivate: [canActivateAuth]},
  { path: 'rooms', component: RoomControlPageComponent, canActivate: [canActivateAuth]}
];
