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

  
  constructor() {
    this.userPageService.getUsers()
      .subscribe(val => {
        this.users = val;
      });
  }
}