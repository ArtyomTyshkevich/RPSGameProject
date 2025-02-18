import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RoomCardComponent } from '../../common-ui/room-card/room-card.component';
import { Room } from '../../cores/models/room';
import { RoomService } from '../../services/room.service';
import { RoomTypes } from '../../cores/enums/roomTypes';
import { RoomStatuses } from '../../cores/enums/roomStatuses';

@Component({
  selector: 'app-rating-page',
  imports: [RoomCardComponent, CommonModule],
  templateUrl: './room-control-page.component.html',
  styleUrls: ['./room-control-page.component.scss']
})
export class RoomControlPageComponent {
  roomService = inject(RoomService);
  rooms: Room[] = [];

  
  constructor() {
    this.roomService.getRooms()
      .subscribe(val => {
        this.rooms = val;
      });
  }
  onCreateRoom() {
    const newRoom: Room = {
      id: null,
      roomType: RoomTypes.Default,
      roomStatus: RoomStatuses.InPreparation
    };
    this.roomService.createRoom(newRoom).subscribe(() => {
    });
    window.location.reload();
  }
  onDeleteInactiveRoom()
  {
    this.roomService.deleteInactiveRooms().subscribe(() => {
    });
    window.location.reload();
  }


}
