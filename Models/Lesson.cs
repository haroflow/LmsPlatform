using System.ComponentModel.DataAnnotations;

namespace LmsPlatform.Models
{
    public class Lesson
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; } = "";
        public string Slug { get; set; } = "";
        public Module? Module { get; set; }

        // VideoLesson
        public string Text { get; set; } = "";
        public string VideoURL { get; set; } = "";
    }
}