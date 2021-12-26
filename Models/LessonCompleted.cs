using System.ComponentModel.DataAnnotations;

namespace LmsPlatform.Models
{
	public class LessonCompleted
	{
		[Key]
		public long Id { get; set; }

		public string? Username { get; set; }

		public long CourseId { get; set; }

		public long LessonId { get; set; }
	}
}