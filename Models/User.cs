using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LmsPlatform.Models
{
    public class User
    {
        [Key]
        [MinLength(5), MaxLength(50)]
		public string Username { get; internal set; } = "";

        [Required]
        [MinLength(5), MaxLength(150)]
        public string Name { get; set; } = "";

        [Required]
        public string PasswordHash { get; set; } = "";

        [Required]
        public UserRole Role { get; set; }

        public List<Course> MyCourses { get; set; } = new List<Course>();
	}
}
