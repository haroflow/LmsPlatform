import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgxBootstrapIconsModule, allIcons } from 'ngx-bootstrap-icons';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { CourseListComponent } from './course-list/course-list.component';
import { CourseCardComponent } from './course-card/course-card.component';
import { CourseDetailsComponent } from './course-details/course-details.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { JwtModule } from '@auth0/angular-jwt';
import { AuthGuardService } from './auth-guard.service';
import { MyCoursesComponent } from './my-courses/my-courses.component';
import { StudyComponent } from './study/study.component';

function tokenGetter() {
  return localStorage.getItem("jwt");
}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    FetchDataComponent,
    CourseListComponent,
    CourseCardComponent,
    CourseDetailsComponent,
    RegisterComponent,
    LoginComponent,
    MyCoursesComponent,
    StudyComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      // Public
      { path: '', component: LoginComponent, pathMatch: 'full' },
      { path: 'register', component: RegisterComponent },
      { path: 'login', component: LoginComponent },

      // Restricted to Students
      { path: 'courses', component: CourseListComponent, canActivate: [AuthGuardService] },
      { path: 'courses/:course', component: CourseDetailsComponent, canActivate: [AuthGuardService] },
      { path: 'courses/:course/study', component: StudyComponent, canActivate: [AuthGuardService] },
      { path: 'courses/:course/:module/:lesson', component: StudyComponent, canActivate: [AuthGuardService] },
      { path: 'my-courses', component: MyCoursesComponent, canActivate: [AuthGuardService] },
    ]),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: [ "localhost:7030" ],
        disallowedRoutes: [],
      }
    }),
    NgxBootstrapIconsModule.pick(allIcons)
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
