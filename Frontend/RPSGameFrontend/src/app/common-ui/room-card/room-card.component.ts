import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Room } from '../../cores/models/room';

@Component({
  selector: 'app-room-card',
  imports: [CommonModule],
  templateUrl: './room-card.component.html',
  styleUrl: './room-card.component.scss'
})
export class RoomCardComponent {
  @Input() room!: Room;
}
