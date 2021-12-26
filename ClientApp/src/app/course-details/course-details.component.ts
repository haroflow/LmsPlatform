import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Course } from 'src/model/Course';
import { CoursesService } from '../courses.service';

@Component({
  selector: 'app-course-details',
  templateUrl: './course-details.component.html',
  styleUrls: ['./course-details.component.css']
})
export class CourseDetailsComponent implements OnInit {
  error: any;
  slug: string = "";
  course?: Course;

  constructor(private coursesService: CoursesService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit(): void {
    this.slug = this.route.snapshot.paramMap.get("course") || "";

    if (this.slug !== "") {
      this.coursesService.getCourseBySlug(this.slug)
        .subscribe(
          course => {
            console.log(course);
            this.course = course
          },
          error => this.error = error);
    }
  }

  totalLessons(course: Course): number {
    return course.modules
      .map(m => m.lessons.length)
      .reduce((p, c) => p + c);
  }

  enroll(course: Course): void {
    this.coursesService.enrollInCourse(course)
      .subscribe(
        result => {
          this.router.navigate(["/courses/", course.slug, "study"]);
        }) // TODO show to the user
  }

  goToCourse(course: Course): void {
    this.router.navigate(["/courses/", course.slug, "study"]);
  }
}
