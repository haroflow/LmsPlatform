import { Component, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { Course } from 'src/model/Course';
import { Lesson } from 'src/model/Lesson';
import { CoursesService } from '../courses.service';

@Component({
  selector: 'app-study',
  templateUrl: './study.component.html',
  styleUrls: ['./study.component.css']
})
export class StudyComponent implements OnInit {
  course?: Course;
  lesson?: Lesson;
  courseSlug: string = "";
  moduleSlug: string = "";
  lessonSlug: string = "";

  constructor(
    private route: ActivatedRoute,
    private coursesService: CoursesService,
    private domSanitizer: DomSanitizer,
    private router: Router) { }

  ngOnInit(): void {
    this.route.url.subscribe(params => this.urlChanged(params))
  }

  urlChanged(params: UrlSegment[]) {
    this.courseSlug = params[1].path;
    this.moduleSlug = params.length > 2 ? params[2].path : "";
    this.lessonSlug = params.length > 3 ? params[3].path : "";

    // TODO Check for empty slugs
    console.log("courseSlug:", this.courseSlug, "moduleSlug:", this.moduleSlug, "lessonSlug:", this.lessonSlug);

    // if (moduleSlug === "" || lessonSlug === "") {
      // Find first non watched lesson and redirect to it.
    //   return;
    // }

    if (!this.course) {
      this.loadCourseInfo();
    }
    
    this.coursesService.getLesson(this.courseSlug, this.moduleSlug, this.lessonSlug)
      .subscribe(lesson => this.lesson = lesson);
  }

  loadCourseInfo() {
    this.coursesService.getMyCourseBySlug(this.courseSlug)
        .subscribe(course => {
          this.course = course
        });
  }

  sanitizeURL(url: string) {
    // TODO Is this secure? Doesn't seem to be.
    return this.domSanitizer.bypassSecurityTrustResourceUrl(url);
  }

  completeAndGoToNextLesson() {
    this.coursesService.lessonComplete(this.lesson!.id)
      .subscribe(result => {
        // TODO change the lesson in place, so we don't need to get from the database?
        this.loadCourseInfo();
      });
  }

}
