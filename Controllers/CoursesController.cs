using LmsPlatform.Dtos;
using LmsPlatform.Repositories;
using LmsPlatform.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LmsPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
		private readonly CourseRepository _courseRepository;
		private readonly UserRepository _userRepository;

		public CoursesController(CourseRepository courseRepository, UserRepository userRepository)
        {
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
                return NotFound("User enrolled courses not found");

            var course = await _courseRepository.GetCourseBySlugAsync(slug);
            if (course == null)
                return NotFound("Course not found");

            var result = new GetCourseDto {
                Name = course.Name,
                Slug = course.Slug,
                Description = course.Description,
                BannerImage = course.BannerImage,
                Modules = course.Modules.OrderBy(m => m.Id).Select(m =>
                    new GetModuleDto()
                    {
                        Title = m.Title,
                        Slug = m.Slug,
                        Lessons = m.Lessons.OrderBy(l => l.Id).Select(l =>
                            new GetLessonDto()
                            {
                                Title = l.Title,
                                Slug = l.Slug,
                            }).ToList()
                    }).ToList(),
                UserEnrolled = enrolledCourses.Any(c => c.Id == course.Id)
            };

            return Ok(result);
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
            var lesson = await _courseRepository
                .GetLessonAsync(courseSlug, moduleSlug, lessonSlug);
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
                Modules = course.Modules.OrderBy(m => m.Id).Select(m =>
                    new GetMyModuleDto
                    {
                        Title = m.Title,
                        Slug = m.Slug,
                        Lessons = m.Lessons.OrderBy(l => l.Id).Select(l =>
                            new GetMyLessonDto
                            {
                                Title = l.Title,
                                Slug = l.Slug,
                                Completed = progress.Any(t => t.LessonId == l.Id)
                            }
                        ).ToList()
                    }).ToList()
            };

            return Ok(result);
        }

        [HttpPost]
        [Route("{lessonId}/completed")]
        [Authorize]
        public async Task<IActionResult> LessonCompleteAsync([FromRoute] long lessonId)
        {
            var username = new AuthUtilities().GetLoggedUsername(this);
            
            var lesson = await _courseRepository.GetLessonByIdAsync(lessonId);
            if (lesson == null || lesson.Module == null || lesson.Module.Course == null)
                return NotFound("Lesson not found");

            var userEnrolled = await _courseRepository
                .IsUserEnrolledInCourseAsync(username, lesson.Module.Course.Id);
            if (!userEnrolled)
                return BadRequest("User not enrolled in this course");

            await _courseRepository.SetLessonAsCompletedAsync(
                username,
                lesson.Module.Course.Id,
                lesson.Id);

            return Ok();
        }
	}
}
