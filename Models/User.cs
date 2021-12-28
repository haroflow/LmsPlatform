using System.ComponentModel.DataAnnotations;

namespace LmsPlatform.Models
{
	public class User
	{
		[Key]
		public string Username { get; set; } = "";

		public string Name { get; set; } = "";

		public string PasswordHash { get; set; } = "";

		public UserRole Role { get; set; }

		public List<Course> Courses { get; set; } = new List<Course>();
	}
}
