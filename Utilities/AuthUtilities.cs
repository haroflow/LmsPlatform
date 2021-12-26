using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace LmsPlatform.Utilities
{
	public class AuthUtilities
	{
		public string? GetLoggedUsername(ControllerBase controller) {
			return controller.User.Claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value;
		}
	}
}