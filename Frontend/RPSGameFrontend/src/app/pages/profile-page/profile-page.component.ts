import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router'; // Для получения параметра из маршрута
import { UserPageService } from '../../services/user-page.service'; // Ваш сервис
import { UserWithStatistics } from '../../cores/requests/UserWithStatistics';

@Component({
  selector: 'app-profile-page',
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.scss']
})
export class ProfilePageComponent implements OnInit {
  userId: string = '';
  user: UserWithStatistics = {} as UserWithStatistics; 
  constructor(
    private userPageService: UserPageService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.userId = this.route.snapshot.paramMap.get('userId') || '';

    this.userPageService.getWithStatistics(this.userId).subscribe((val) => {
      this.user = val;
    });
  }
}
