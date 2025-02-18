import { Component, inject, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Room } from '../../cores/models/room';
import { RoomTypes } from '../../cores/enums/roomTypes';
import { RoomStatuses } from '../../cores/enums/roomStatuses';
import { RoomService } from '../../services/room.service';
import { AuthService } from '../../services/auth-service.service';

@Component({
  selector: 'app-room-card',
  imports: [CommonModule],
  templateUrl: './room-card.component.html',
  styleUrl: './room-card.component.scss'
})
export class RoomCardComponent {
  roomService = inject(RoomService);
  authService = inject(AuthService);
  roomTypes = RoomTypes;
  roomStatuses = RoomStatuses;
  @Input() room!: Room;
 
  onDelete() {
    this.roomService.deleteRoom(this.room.id!).subscribe(() => {
      console.log(`Room ${this.room.id} deleted successfully.`);
      window.location.reload(); 
    });
  }
  
  onToggleStatus() {
    const newStatus = this.room.roomStatus === RoomStatuses.Inactive 
      ? RoomStatuses.InPreparation 
      : RoomStatuses.Inactive;
  
    this.roomService.updateRoomStatus(this.room.id!, newStatus).subscribe(() => {
      console.log(`Room ${this.room.id} status updated to ${newStatus}`);
      this.room.roomStatus = newStatus;
      window.location.reload(); 
    });
  }
  
}

