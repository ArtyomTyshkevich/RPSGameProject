import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router'; 
import { UserPageService } from '../../services/user-page.service'; 
import { UserWithStatistics } from '../../cores/requests/UserWithStatistics';
import { AuthService } from '../../services/auth-service.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-profile-page',
  imports: [CommonModule],
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.scss']
})
export class ProfilePageComponent implements OnInit {
  authService = inject(AuthService);
  router = inject(Router);
  userId: string = '';
  user: UserWithStatistics = {} as UserWithStatistics; 
  isAdminOrCurrentUser: boolean = false;
  showDeleteConfirmation: boolean = false; // State for modal confirmation

  constructor(
    private userPageService: UserPageService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.userId = this.route.snapshot.paramMap.get('userId') || '';
    if( this.authService.getUserRoleFromToken() == "admin" || 
        this.userId == this.authService.getUserIdFromToken()) {
      this.isAdminOrCurrentUser = true;
    }

    this.userPageService.getWithStatistics(this.userId).subscribe((val) => {
      this.user = val;
    });
  }

  confirmDelete() {
    this.showDeleteConfirmation = true;
  }

  cancelDelete() {
    this.showDeleteConfirmation = false;
  }

  deleteUser() {
    this.userPageService.deleteUser(this.userId).subscribe(() => {
      this.authService.logout();
    });
  }
}
