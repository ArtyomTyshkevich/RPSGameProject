import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../enviroment/environment';

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  private http = inject(HttpClient);
  private baseUrl = environment.searchService;

  Start(id: string): Observable<string> {
    const url = `${this.baseUrl}/search/${id}/start`;
    return this.http.post<string>(url, {});
  }

  Stop(id: string): Observable<string> {
    const url = `${this.baseUrl}/search/${id}/stop`;
    return this.http.post<string>(url, {});
  }
}
