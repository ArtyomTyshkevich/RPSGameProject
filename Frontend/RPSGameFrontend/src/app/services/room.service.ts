import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Room } from '../cores/models/room';
import { RoomStatuses } from '../cores/enums/roomStatuses';
import { environment } from '../enviroment/environment';

@Injectable({
  providedIn: 'root'
})
export class RoomService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.roomServiceUrl}/rooms`;

  getRooms(): Observable<Room[]> {
    return this.http.get<Room[]>(this.baseUrl);
  }

  getRoomById(id: string): Observable<Room> {
    return this.http.get<Room>(`${this.baseUrl}/${id}`);
  }

  createRoom(room: Room): Observable<void> {
    return this.http.post<void>(this.baseUrl, room);
  }

  deleteRoom(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  deleteInactiveRooms(): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/inactive`);
  }

  updateRoomStatus(roomId: string, newStatus: RoomStatuses): Observable<void> {
    return this.http.patch<void>(`${this.baseUrl}/${roomId}/status?newStatus=${newStatus}`, {});
  }
}