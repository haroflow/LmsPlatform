using System.ComponentModel.DataAnnotations;

namespace LmsPlatform.Models
{
    public class Module
    {
        [Key]
        public long Id {  get; set; }
        public string Title { get; set; } = "";
		public string Slug { get; set; } = "";
        public long CourseId { get; set; }
        public Course? Course { get; set; }

        public List<Lesson> Lessons { get; set; } = new List<Lesson>();
	}
}
