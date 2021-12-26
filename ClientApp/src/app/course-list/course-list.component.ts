import { Component, OnInit } from '@angular/core';
import { Course } from 'src/model/Course';
import { CoursesService } from '../courses.service';

@Component({
  selector: 'app-course-list',
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.css']
})
export class CourseListComponent implements OnInit {
  courses: Course[] = [];

  constructor(private coursesService: CoursesService) { }

  ngOnInit(): void {
    this.coursesService.getAllCourses()
      .subscribe(courses => this.courses = courses);
  }

}


