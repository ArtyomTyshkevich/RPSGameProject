import { Component, Input } from '@angular/core';
import { User } from '../../cores/models/user';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-card',
  imports: [CommonModule],
  templateUrl: './user-card.component.html',
  styleUrl: './user-card.component.scss'
})
export class UserCardComponent {
  @Input() index!: number;
  @Input() user!: User;
}
