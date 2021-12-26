import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Course } from 'src/model/Course';
import { CoursesService } from '../courses.service';

@Component({
  selector: 'app-my-courses',
  templateUrl: './my-courses.component.html',
  styleUrls: ['./my-courses.component.css']
})
export class MyCoursesComponent implements OnInit {
  errorMessage?: string;
  courses: Course[] = [];

  constructor(private coursesServices: CoursesService, private jwtHelper: JwtHelperService) { }

  ngOnInit(): void {
    this.coursesServices.getMyCourses()
      .subscribe(
        courses => this.courses = courses,
        err => {
          console.error(err);
          this.errorMessage = "Failed to load My Courses"
        });
  }

}
