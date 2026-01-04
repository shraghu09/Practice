import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'http://localhost:5048/api/auth';

  constructor(private http: HttpClient) {}

  login(email: string, password: string) {
    return this.http.post<any>(
      `${this.apiUrl}/login`,
      { email, password }
    );
  }

  getUsers() {
    const token = localStorage.getItem('token');
    return this.http.get<any[]>(
      `${this.apiUrl}/users`,
      {
        headers: {
          Authorization: `Bearer ${token}`
        }
      }
    );
  }
}
