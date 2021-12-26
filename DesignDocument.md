# Design Document

## Back-end

Main language: C#/ASP.Net Core.
Data storage: To begin we'll be using Entity Framework Core for data persistence, using an In-Memory database.
On deploy it must be configurable to use another database server.

## Front-end

Angular 12

## Objectives

I'm using this project as a way to learn ASP.Net/EF/Angular, so don't expect it to be usable in production.

My main objective is to create a simple LMS platform comprising of:

- Creation of courses, with lessons of different types:
	- Videos hosted on Youtube (private) or Vimeo.
	- Text lesson
	- Quiz
	- Section for students questions
- Log-in for students, teachers and administrators using a simple username and password.
- Administrator area
- Teacher area
- Student area
- All courses free for the moment. No checkout/payment implementation.
- Certificate of completion for a course.

## Definitions

Student: Registered user, with a role of Student. Can enroll in courses.
Teacher: Registered user, with a role of Teacher. Can create courses.
Administrator: Registered user, with a role of Administrator. Can manage the platform and create Teachers.
Course: Created by Teacher, has one or more modules. Students may enroll in a course.
Module: Groups one or more lessons in a logical section.
Lesson: An individual lesson inside a Module. May be a Video, Text or Quiz.
Certificate of conclusion: Students receive a certificate on course conclusion. May be downloaded as PDF.

## User roles

- Administrator
	- Can add new teacher credentials.
	- Can view analytics for all the courses and individual teachers.
	- Can view comments and questions.
- Teacher
	- Can create courses and lessons.
	- Can view analytics for their own courses.
	- Can view and answer comments and questions of their own courses.
- Student
	- Can view the course listing.
	- Can enroll in a course for free.
	- Can view lessons in enrolled courses.
	- Can view their progress for enrolled courses.
	- Can add a comment or question to a lesson.

A single user can have multiple roles:
- Student
- Student and Teacher
- Student, Teacher and Administrator.

## Routes

### Public
- /login
- /registration - registration via username and password

### Authenticated Administrators, Teachers and Students
- /profile - user profile
- /my-courses - user's enrolled courses
- /courses - lists all available courses
- /courses/{slug} - course details
- /courses/{slug}/enroll - enroll user in the course
- /courses/{slug}/study - shows the first non-watched lesson of the course
?- /courses/{slug}/{module}/ - module intro
?- /courses/{slug}/{module}/{lesson} - lesson page
- /certificates - user's certificates
- /certificates/{slug} - certificate info, export to PDF, etc
- /teacher/{id} - teacher's public info, other courses offered by them, etc

### Authenticated Teachers
- /teacher/create-course
- /teacher/edit-course/{id}

### Authenticated Administrators
- /admin - administration page
- /admin/teacher

## Lesson structure
- Text only, possibly with Markdown support.
- Video, with accompanying text
- Quiz
	- Multiple choice
	- Objective