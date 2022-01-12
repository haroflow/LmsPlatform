using LmsPlatform.Data;
using LmsPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace LmsPlatform.Repositories
{
	public class CourseRepository
	{
		private readonly AppDbContext _context;

		public CourseRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Course>> GetAllCoursesAsync()
		{
			var list = await _context.Courses.ToListAsync();
			return list;
		}

		public async Task<Course?> GetCourseBySlugAsync(string slug)
		{
			var course = await _context.Courses
				.Include(c => c.Modules)
				.ThenInclude(m => m.Lessons)
				.AsSplitQuery()
                .FirstOrDefaultAsync(c => c.Slug == slug);

			return course;
		}

		public async Task<bool> EnrollStudentInCourseAsync(string username, string courseSlug)
		{
			var user = await _context.Users.FindAsync(username);
            if (user == null)
                return false;

            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Slug == courseSlug);
            if (course == null)
                return false;

			user.Courses.Add(course);
            await _context.SaveChangesAsync();

			return true;
		}

		public async Task<Lesson?> GetLessonAsync(string courseSlug, string moduleSlug, string lessonSlug)
		{
			return await _context.Lessons.FirstOrDefaultAsync(l =>
                l.Slug == lessonSlug &&
                l.Module != null &&
				l.Module.Slug == moduleSlug &&
				l.Module.Course != null &&
                l.Module.Course.Slug == courseSlug);
		}

		public async Task<List<LessonCompleted>?> GetUserCourseProgressAsync(string? username, long courseId)
		{
			var progress = await _context.LessonCompleted
				.Where(t => t.Username == username && t.CourseId == courseId)
				.ToListAsync();

			return progress;
		}

		public async Task<bool> SetLessonAsCompletedAsync(string username, long courseId, long lessonId)
		{
			var lc = new LessonCompleted
			{
				Username = username,
				LessonId = lessonId,
				CourseId = courseId
			};
            _context.LessonCompleted.Add(lc);
            var changed = await _context.SaveChangesAsync();

			return changed > 0;
		}

		public async Task<Lesson?> GetLessonByIdAsync(long lessonId)
		{
			return await _context.Lessons
				.Include(l => l.Module)
				.ThenInclude(m => m!.Course)
				.FirstOrDefaultAsync(l => l.Id == lessonId);
		}

		public async Task<bool> IsUserEnrolledInCourseAsync(string username, long courseId)
		{
			var user = await _context.Users
				.Include(u => u.Courses)
				.AsNoTracking()
				.FirstOrDefaultAsync(u => u.Username == username);
			if (user == null)
				return false;
			
			var isEnrolled = user.Courses
				.Any(c => c.Id == courseId);

			return isEnrolled;
		}
	}
}