import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserPageService {
  http = inject(HttpClient)

  getUsers(): void {
    this.http.get(url: )
  }
}
