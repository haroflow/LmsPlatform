import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  invalidLogin = false;

  registerForm = this.formBuilder.group({
    username: '',
    password: '',
  });

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router) { }

  ngOnInit(): void {
    // Redirect authenticated users to 'My Courses' page
    // TODO: If the user is not enrolled in any course, we could redirect to /courses
    if (this.authService.isAuthenticated())
      this.router.navigate([ "/my-courses" ]);
  }

  onSubmit(): void {
    var username = this.registerForm.get("username")?.value;
    var password = this.registerForm.get("password")?.value;

    this.authService.loginUser(username, password)
      .subscribe(
        response => {
          this.invalidLogin = false;

          var token = (<any>response).token;
          localStorage.setItem("jwt", token);

          this.router.navigate(["courses"]);
        },
        error => {
          this.invalidLogin = true;
        });
  }

}
