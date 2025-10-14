import { Component, inject } from '@angular/core';
import { UserCardComponent } from '../../common-ui/user-card/user-card.component';
import { UserPageService } from '../../services/user-page.service';
import { User } from '../../cores/models/user';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-rating-page',
  imports: [UserCardComponent, CommonModule],
  templateUrl: './rating-page.component.html',
  styleUrls: ['./rating-page.component.scss']
})
export class RatingPageComponent {
  userPageService: UserPageService = inject(UserPageService);
  users: User[] = [];
  currentPage = 1;
  pageSize = 8;
  totalUserCount = 0;

  get totalPages() {
    return Math.ceil(this.totalUserCount / this.pageSize);
  }

  get pages() {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  goToPage(page: number) {
    this.currentPage = page;
    this.loadUsers();
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadUsers();
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadUsers();
    }
  }

  loadUsers() {
    this.userPageService.getUsersSortedByRating(this.currentPage, this.pageSize)
      .subscribe({
        next: (val) => {
          this.users = val;
        },
        error: (err) => {
          console.error('Ошибка при загрузке пользователей:', err);
        }
      });

    this.userPageService.getTotalUserCount()
      .subscribe({
        next: (count) => {
          this.totalUserCount = count;
        },
        error: (err) => {
          console.error('Ошибка при получении общего количества пользователей:', err);
        }
      });
  }

  constructor() {
    this.loadUsers(); 
  }
}
