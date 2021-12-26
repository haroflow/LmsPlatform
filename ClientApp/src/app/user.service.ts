import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl = "https://localhost:7030";

  constructor(private http: HttpClient) { }

  checkUsernameAvailability(username: string): Observable<CheckUsernameAvailability> {
    return this.http
      .get<CheckUsernameAvailability>(`${this.baseUrl}/api/users/check-username-availability?username=${username}`);
  }

  registerUser(username: string, password: string) {
    return this.http.post(`${this.baseUrl}/api/users`, {
      username: username,
      password: password,
    });
  }
}

interface CheckUsernameAvailability {
  available: boolean;
}
