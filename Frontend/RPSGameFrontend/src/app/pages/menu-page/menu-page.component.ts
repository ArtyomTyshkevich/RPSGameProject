import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth-service.service';
import { Router } from '@angular/router';
import { MenuStates } from '../../cores/enums/MenuStates';
import { SearchService } from '../../services/search.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-menu-page',
  imports: [RouterLink, CommonModule],
  templateUrl: './menu-page.component.html',
  styleUrl: './menu-page.component.scss'
})
export class MenuPageComponent {
  authService = inject(AuthService);
  searchService = inject(SearchService);
  router = inject(Router);
  pageState: MenuStates = MenuStates.MainMenu;
  menuStates = MenuStates;
  isLoading: boolean = false;
  userId: string = this.authService.getUserIdFromToken();

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/start']);
  }

  startGameSearch(): void {


    this.isLoading = true;
    this.pageState = MenuStates.InSearch;

    this.searchService.Start(this.userId).subscribe({
      next: (response: string) => {
        console.log('Поиск начат:', response);
        this.isLoading = false;
        this.router.navigate(['/game', response]);
      },
      error: (error: any) => {
        console.error('Ошибка при начале поиска:', error);
        this.pageState = MenuStates.MainMenu;
        this.isLoading = false;
      }
    });
  }
}