using System.ComponentModel.DataAnnotations;

namespace LmsPlatform.Dtos
{
	public class CheckUsernameAvailabilityRequest
	{
		[Required]
		public string Username { get; set; } = "";
	}
}