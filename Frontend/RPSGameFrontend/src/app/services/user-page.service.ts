import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../cores/models/user';

@Injectable({
  providedIn: 'root',
})
export class UserPageService {
  private http = inject(HttpClient);

  getUsers(): Observable<User[]> {
    const url = 'https://localhost:8095/users';
    return this.http.get<User[]>(url);
  }
}