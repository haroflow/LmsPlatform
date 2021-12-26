using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LmsPlatform.Models
{
    public class Module
    {
        [Key]
        public long Id {  get; set; }

        [Required]
        public string Title { get; set; } = "";
        
        [Required]
		public string Slug { get; set; } = "";

        public List<Lesson> Lessons { get; set; } = new List<Lesson>();

        public Course? Course { get; set; }
	}
}
