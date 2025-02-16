import { Component, inject, OnDestroy } from '@angular/core';
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
  styleUrls: ['./menu-page.component.scss']
})
export class MenuPageComponent implements OnDestroy {
  authService = inject(AuthService);
  searchService = inject(SearchService);
  router = inject(Router);
  pageState: MenuStates = MenuStates.MainMenu;
  menuStates = MenuStates;
  roomId: string = '';
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
      next: (roomId: string) => {
        this.isLoading = false;
        this.roomId = roomId;
        console.log('Рума найдена:', this.roomId);
        this.router.navigate(["/game", this.roomId]);
      },
      error: (error: any) => {
        console.error('Ошибка при начале поиска:', error);
        this.pageState = MenuStates.MainMenu;
        this.isLoading = false;
      }
    });
  }

  stopGameSearch(): void {
    this.isLoading = false;
    this.pageState = MenuStates.MainMenu;

    this.searchService.Stop(this.userId).subscribe({
      next: (response: string) => {
        console.log('Поиск прикращен:', response);
        this.isLoading = false;
      },
      error: (error: any) => {
        console.error('Ошибка при окончании поиска:', error);
        this.pageState = MenuStates.MainMenu;
        this.isLoading = false;
      }
    });
  }

  ngOnDestroy(): void {
    if (this.pageState === MenuStates.InSearch) {
      this.stopGameSearch();
    }
  }
}
