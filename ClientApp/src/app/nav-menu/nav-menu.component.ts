import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  get isUserAuthenticated() {
    return this.authService.isAuthenticated();
  }

  get isTeacher() {
    return this.authService.isTeacher();
  }

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {}

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout() {
    this.authService.logoutUser();
    this.router.navigate([""]);
  }
}
