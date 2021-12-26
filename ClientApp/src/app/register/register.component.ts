import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, ValidationErrors, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { debounceTime, map } from 'rxjs/operators';
import { UserService } from '../user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registerForm = this.formBuilder.group({
    username: [
      '',
      [Validators.required, Validators.minLength(5)],
      this.checkUsernameAvailability(this.userService)
    ],
    password: ['', Validators.required],
    confirmPassword: ['', Validators.required],
  }, {
    validators: [this.checkPasswordConfirmation]
  });
  error: any;

  constructor(private formBuilder: FormBuilder,
    private userService: UserService,
    private router: Router) { }

  ngOnInit(): void {
  }

  checkUsernameAvailability(userService: UserService) {
    return function (control: AbstractControl) {
      const username = control?.value;

      // TODO add debounce
      return userService.checkUsernameAvailability(username)
        .pipe(map(check => (check.available ? null : { alreadyRegistered : true })));
    }
  }

  checkPasswordConfirmation(control: AbstractControl) {
    const password = control.get('password')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;

    if (password === confirmPassword)
      return null;
    else
      return { passwordConfirmationWrong: true };
  }

  onSubmit(): void {
    this.error = null;

    this.userService.registerUser(
      this.username?.value,
      this.password?.value,
    ).subscribe(
      result => {
        this.router.navigate(["/login"]);
      },
      error => this.error = error.error.detail);
  }

  get username() { return this.registerForm.get('username'); }
  get password() { return this.registerForm.get('password'); }
  get confirmPassword() { return this.registerForm.get('confirmPassword'); }

}
