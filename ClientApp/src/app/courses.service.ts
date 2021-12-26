import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Course } from 'src/model/Course';
import { Lesson } from 'src/model/Lesson';

@Injectable({
  providedIn: 'root'
})
export class CoursesService {
  private url = "https://localhost:7030/api/courses";

  constructor(private http: HttpClient) { }

  getAllCourses(): Observable<Course[]> {
    return this.http.get<Course[]>(this.url);
  }

  getCourseBySlug(slug: string): Observable<Course> {
    return this.http.get<Course>(`${this.url}/${slug}`);
  }

  getMyCourses(): Observable<Course[]>{
    return this.http.get<Course[]>(`${this.url}/my-courses`);
  }

  getMyCourseBySlug(slug: string): Observable<Course> {
    return this.http.get<Course>(`${this.url}/my-courses/${slug}`);
  }

  enrollInCourse(course: Course): Observable<any> {
    return this.http.post<any>(`${this.url}/${course.slug}/enroll`, {});
  }

  getLesson(courseSlug: string,
    moduleSlug: string,
    lessonSlug: string): Observable<Lesson> {
      
    return this.http
      .get<Lesson>(`${this.url}/${courseSlug}/${moduleSlug}/${lessonSlug}`);
  }

  lessonComplete(lessonId: string): Observable<any> {
    return this.http.post<any>(`${this.url}/${lessonId}/completed`, {});
  }
}
