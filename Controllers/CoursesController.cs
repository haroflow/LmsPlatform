using System.Security.Claims;
using LmsPlatform.Data;
using LmsPlatform.Dtos;
using LmsPlatform.Models;
using LmsPlatform.Repositories;
using LmsPlatform.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LmsPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
		private readonly DataContext _context;
		private readonly CourseRepository _courseRepository;
		private readonly UserRepository _userRepository;

		public CoursesController(DataContext context, CourseRepository courseRepository, UserRepository userRepository)
        {
            _context = context;
            _courseRepository = courseRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<IActionResult> GetAllCourses()
        {
            var username = new AuthUtilities().GetLoggedUsername(this);

            var enrolledCourses = await _userRepository.GetUserEnrolledCoursesAsync(username);
            if (enrolledCourses == null)
                return NotFound("User not found");

            var list = (await _courseRepository.GetAllCoursesAsync())
                .Select(course => new GetAllCoursesResponse {
                    MiniatureImage = course.MiniatureImage,
                    Slug = course.Slug,
                    Name = course.Name,
                    Description = course.Description,
                    UserEnrolled = enrolledCourses.Any(c => c.Id == course.Id)
                });

            return Ok(list);
        }

        [HttpGet]
        [Route("{slug}")]
        [Authorize]
        public async Task<IActionResult> GetCourseBySlug([FromRoute] string slug)
        {
            var username = new AuthUtilities().GetLoggedUsername(this);

            var enrolledCourses = await _userRepository.GetUserEnrolledCoursesAsync(username);
            if (enrolledCourses == null)
                return NotFound("User not found");

            var course = await _courseRepository.GetCourseBySlugAsync(slug);
            if (course == null)
                return NotFound("Course not found");

            var ret = new GetCourseDto {
                Name = course.Name,
                Slug = course.Slug,
                Description = course.Description,
                BannerImage = course.BannerImage,
                Modules = new List<GetModuleDto>(),
                UserEnrolled = enrolledCourses.Any(c => c.Id == course.Id)
            };

            foreach (Module module in course.Modules.OrderBy(m => m.Id)) {
                var m = new GetModuleDto {
                    Title = module.Title,
                    Slug = module.Slug,
                    Lessons = new List<GetLessonDto>(),
                };

                foreach (Lesson lesson in module.Lessons.OrderBy(l => l.Id)) {
                    m.Lessons.Add(new GetLessonDto {
                        Title = lesson.Title,
                        Slug = lesson.Slug,
                    });
                }

                ret.Modules.Add(m);
            }

            return Ok(ret);
        }

        [HttpGet]
        [Route("my-courses")]
        [Authorize]
        public async Task<IActionResult> GetMyCourses()
        {
            var username = new AuthUtilities().GetLoggedUsername(this);

            var enrolledCourses = await _userRepository.GetUserEnrolledCoursesAsync(username);
            if (enrolledCourses == null)
                return NotFound("User not found");

            var result = enrolledCourses.Select(course => new GetAllCoursesResponse {
                MiniatureImage = course.MiniatureImage,
                Slug = course.Slug,
                Name = course.Name,
                Description = course.Description,
                UserEnrolled = true
            });

            return Ok(result);
        }

        [HttpPost]
        [Route("{courseSlug}/enroll")]
        [Authorize]
        public async Task<IActionResult> EnrollStudentInCourse([FromRoute] string courseSlug)
        {
            var username = new AuthUtilities().GetLoggedUsername(this);
            var saved = await _courseRepository.EnrollStudentInCourseAsync(username, courseSlug);

            return saved ?
                Ok() :
                NotFound();
        }

        [HttpGet]
        [Route("{courseSlug}/{moduleSlug}/{lessonSlug}")]
        [Authorize]
        public async Task<IActionResult> GetLesson(
            [FromRoute] string courseSlug,
            [FromRoute] string moduleSlug,
            [FromRoute] string lessonSlug)
        {
            var lesson = await _courseRepository.GetLessonAsync(courseSlug, moduleSlug, lessonSlug);
            if (lesson == null)
                return NotFound("Lesson not found");

            return Ok(lesson);
        }

        [HttpGet]
        [Route("my-courses/{courseSlug}")]
        [Authorize]
        public async Task<IActionResult> GetMyCourseBySlug([FromRoute] string courseSlug)
        {
            var username = new AuthUtilities().GetLoggedUsername(this);

            var user = await _userRepository.GetUser(username);
            if (user == null)
                return NotFound("User not found");

            var course = await _courseRepository.GetCourseBySlugAsync(courseSlug);
            if (course == null)
                return NotFound("Course not found");

            var progress = await _courseRepository.GetUserCourseProgressAsync(username, course.Id);
            if (progress == null)
                return NotFound("Course progress not found");

            var result = new GetMyCourseDto {
                Name = course.Name,
                Slug = course.Slug,
                Description = course.Description,
                BannerImage = course.BannerImage,
                Modules = new List<GetMyModuleDto>(),
            };

            foreach (Module module in course.Modules.OrderBy(m => m.Id)) {
                var m = new GetMyModuleDto {
                    Title = module.Title,
                    Slug = module.Slug,
                    Lessons = new List<GetMyLessonDto>(),
                };

                foreach (Lesson lesson in module.Lessons.OrderBy(l => l.Id)) {
                    m.Lessons.Add(new GetMyLessonDto {
                        Title = lesson.Title,
                        Slug = lesson.Slug,
                        Completed = progress.Any(t => t.LessonId == lesson.Id)
                    });
                }

                result.Modules.Add(m);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("{lessonId}/completed")]
        [Authorize]
        public async Task<IActionResult> LessonCompleteAsync([FromRoute] long lessonId)
        {
            var username = new AuthUtilities().GetLoggedUsername(this);

            var user = await _userRepository.GetUser(username);
            if (user == null)
                return NotFound("User not found");

            var lesson = await _courseRepository.GetLessonByIdAsync(lessonId);
            if (lesson == null || lesson.Module == null || lesson.Module.Course == null)
                return NotFound("Lesson not found");

            var userEnrolled = await _courseRepository.IsUserEnrolledInCourseAsync(username, lesson.Module.Course.Id);
            if (!userEnrolled)
                return BadRequest("User not enrolled in this course");

            var saved = await _courseRepository.SetLessonAsCompletedAsync(
                username,
                lesson.Module!.Course!.Id,
                lesson.Id);

            return Ok();
        }

        [HttpGet]
        [Route("completed")]
        [Authorize]
        public IActionResult GetLessonComplete()
        {
            // TODO is this in use?
            var username = new AuthUtilities().GetLoggedUsername(this);

            var user = _userRepository.GetUser(username);
            if (user == null)
                return NotFound("User not found");

            var l = _context.LessonsCompleted.ToList();

            return Ok(l);
        }
	}
}
