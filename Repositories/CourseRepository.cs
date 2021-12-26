using LmsPlatform.Data;
using LmsPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace LmsPlatform.Repositories
{
	public class CourseRepository
	{
		private readonly DataContext _context;

		public CourseRepository(DataContext context)
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
			return await _context.Courses
                .Include(c => c.Modules)
                .ThenInclude(m => m.Lessons)
                .FirstOrDefaultAsync(c => c.Slug == slug);
		}

		public async Task<bool> EnrollStudentInCourseAsync(string? username, string courseSlug)
		{
			var u = await _context.Users.FindAsync(username);
            if (u == null)
                return false;

            var c = await _context.Courses.FirstOrDefaultAsync(c => c.Slug == courseSlug);
            if (c == null)
                return false;

            u.MyCourses.Add(c);
            await _context.SaveChangesAsync();

			return true;
		}

		public async Task<Lesson?> GetLessonAsync(string courseSlug, string moduleSlug, string lessonSlug)
		{
			return await _context.Lessons.FirstOrDefaultAsync(l =>
                l.Slug == lessonSlug &&
                l.Module!.Slug == moduleSlug &&
                l.Module!.Course!.Slug == courseSlug);
		}

		// public async Task<Course?> GetUserMyCoursesBySlug(string? username, string courseSlug)
		// {
		// 	var course = await _context.Courses
        //         .Include(c => c.Modules)
        //         .ThenInclude(m => m.Lessons)
        //         .FirstOrDefaultAsync(c => c.Slug == courseSlug);
                
		// 	return course;
		// }

		public async Task<List<LessonCompleted>?> GetUserCourseProgressAsync(string? username, long courseId)
		{
			var progress = await _context.LessonsCompleted
				.Where(t => t.Username == username && t.CourseId == courseId)
				.ToListAsync();

			return progress;
		}

		public async Task<bool> SetLessonAsCompletedAsync(string? username, long courseId, long lessonId)
		{
			var lc = new LessonCompleted();
            lc.Username = username;
            lc.LessonId = lessonId;
            lc.CourseId = courseId;
            _context.LessonsCompleted.Add(lc);
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

		public async Task<bool> IsUserEnrolledInCourseAsync(string? username, long id)
		{
			var user = await _context.Users.Include(u => u.MyCourses).FirstOrDefaultAsync(u => u.Username == username);
			if (user == null)
				return false;
			
			var isEnrolled = user.MyCourses.Any(c => c.Id == id);
			return isEnrolled;
		}
	}
}