using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LmsPlatform.Data;
using LmsPlatform.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LmsPlatform.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly DataContext _context;

		public AuthController(DataContext context)
		{
			_context = context;
		}

		[HttpPost]
		public IActionResult Auth([FromBody] AuthDto data)
		{
			if (data == null)
				return BadRequest();

			var u = _context.Users.FirstOrDefault(u => u.Username == data.Username);
			if (u == null)
				return Unauthorized();
			
			if (BCrypt.Net.BCrypt.Verify(data.Password, u.PasswordHash))
			{
				var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretKeysecretKey"));
				var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

				var claims = new List<Claim> {
					new Claim(ClaimTypes.Name, u.Name),
					new Claim(ClaimTypes.NameIdentifier, u.Username),
					new Claim(ClaimTypes.Role, u.Role.ToString()),
				};

				var tokenOptions = new JwtSecurityToken(
					issuer: "https://localhost:7030",
					audience: "https://localhost:7030",
					claims: claims,
					expires: DateTime.Now.AddMinutes(500),
					signingCredentials: signingCredentials
				);

				var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
				return Ok(new {
					Token = tokenString
				});
			}

			return Unauthorized();
		}
	}
}