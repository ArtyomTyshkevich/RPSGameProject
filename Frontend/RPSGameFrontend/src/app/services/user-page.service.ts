import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../cores/models/user';
import { UserWithStatistics } from '../cores/requests/UserWithStatistics';
import { environment } from '../enviroment/environment';

@Injectable({
  providedIn: 'root',
})
export class UserPageService {
  private baseUrl = `${environment.userServiceUrl}/users`;

  constructor(private http: HttpClient) {}

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl);
  }

  getUsersSortedByRating(page: number, pageSize: number): Observable<User[]> {
    const url = `${this.baseUrl}/sorted-by-rating?page=${page}&pageSize=${pageSize}`;
    return this.http.get<User[]>(url);
  }

  getWithStatistics(userId: string): Observable<UserWithStatistics> {
    const url = `${this.baseUrl}/${userId}/statistics`;
    return this.http.get<UserWithStatistics>(url);
  }

  getUserById(userId: string): Observable<User> {
    const url = `${this.baseUrl}/${userId}`;
    return this.http.get<User>(url);
  }

  updateUser(user: User): Observable<void> {
    return this.http.put<void>(this.baseUrl, user);
  }

  deleteUser(userId: string): Observable<void> {
    const url = `${this.baseUrl}/${userId}`;
    return this.http.delete<void>(url);
  }

  getTotalUserCount(): Observable<number> {
    const url = `${this.baseUrl}/total-count`;
    return this.http.get<number>(url);
  }
}