using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LmsPlatform.Models
{
	public class LessonCompleted
	{
		[Key]
		public long Id { get; set; }

        [ForeignKey("User")]
        public string Username { get; set; } = "";
        public User? User { get; set; }

        public long CourseId { get; set; }
        public Course? Course { get; set; }

        public long LessonId { get; set; }
        public Lesson? Lesson { get; set; }
	}

}