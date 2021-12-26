import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Course } from 'src/model/Course';
import { CoursesService } from '../courses.service';

@Component({
  selector: 'app-course-card',
  templateUrl: './course-card.component.html',
  styleUrls: ['./course-card.component.css']
})

export class CourseCardComponent implements OnInit {
  @Input() course?: Course;

  constructor(private coursesService: CoursesService, private router: Router) { }

  ngOnInit(): void {
  }

  enroll(course: Course): void {
    this.coursesService.enrollInCourse(course)
      .subscribe(
        result => {
          if (this.course)
            this.course.userEnrolled = true;
        })
  }
}
