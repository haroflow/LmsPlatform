import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient, private jwtHelper: JwtHelperService) { }

  loginUser(username: string, password: string): Observable<any> {
    return this.http.post("https://localhost:7030/api/auth", {
      username,
      password,
    });
  }

  isAuthenticated(): boolean {
    var token = this.getToken();
    return token !== null && !this.jwtHelper.isTokenExpired(token);
  }

  logoutUser() {
    localStorage.removeItem("jwt");
  }

  getToken(): string | null {
    return localStorage.getItem("jwt");
  }

  isTeacher(): boolean {
    var token = this.getToken();
    if (token == null)
      return false;

    var decodedToken = this.jwtHelper.decodeToken<any>(token);
    var role = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
    return role === "teacher";
  }
}
