import { Component, inject } from '@angular/core';
import { UserPageService } from '../../services/user-page.service';
import { CommonModule } from '@angular/common';
import { RoomCardComponent } from '../../common-ui/room-card/room-card.component';
import { Room } from '../../cores/models/room';
import { RoomService } from '../../services/room.service';

@Component({
  selector: 'app-rating-page',
  imports: [RoomCardComponent, CommonModule],
  templateUrl: './room-control-page.component.html',
  styleUrls: ['./room-control-page.component.scss']
})
export class RoomControlPageComponent {
  roomSerivece = inject(RoomService);
  rooms: Room[] = [];

  
  constructor() {
    this.roomSerivece.getRooms()
      .subscribe(val => {
        this.rooms = val;
      });
  }
}