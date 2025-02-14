import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Room } from '../cores/models/room';

@Injectable({
  providedIn: 'root'
})
export class RoomService {
  private http = inject(HttpClient);

    getRooms(): Observable<Room[]> {
      const url = 'https://localhost:8092/rooms';
      return this.http.get<Room[]>(url);
    }
}
