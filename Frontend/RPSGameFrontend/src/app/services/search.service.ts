import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../cores/models/user';

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  private http = inject(HttpClient);

  Start(id: string): Observable<string> {
    const url = `https://localhost:8092/search/${id}/start`;
    return this.http.post<string>(url, {});
  }
}
