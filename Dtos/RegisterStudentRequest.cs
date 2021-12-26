using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LmsPlatform.Dtos
{
	public class RegisterStudentRequest
    {
        [Required]
        public string Username { get; set; } = "";
        
        [Required]
        // Plain text password
        public string Password { get; set; } = "";
    }
}