﻿using System.ComponentModel.DataAnnotations;

namespace LmsPlatform.Models
{
    public class Course
    {
        [Key]
        public long Id {  get; set; }

        // TODO should be used in urls to reference the course. Should also be unique.
        // Maybe use the format 00000-course-name-without-special-characters.
        [Required]
        public string Slug { get; set; } = "";

        [Required]
        public string Name { get; set; } = "";

        [Required]
        public string Description { get; set; } = "";

        // TODO Store images as base64 string?
        public string BannerImage { get; set; } = "";
        public string MiniatureImage { get; set; } = "";

        public List<Module> Modules { get; set; } = new List<Module>();
    }
}
