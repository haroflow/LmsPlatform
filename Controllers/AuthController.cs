using LmsPlatform.Dtos;
using LmsPlatform.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LmsPlatform.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
        private readonly UserRepository _userRepository;

        public AuthController(UserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		[HttpPost]
		public async Task<IActionResult> AuthAsync([FromBody] AuthDto data)
		{
			if (data == null)
				return BadRequest();

			var user = await _userRepository.GetUserAsync(data.Username);
			if (user == null)
				return Unauthorized();
			
			if (BCrypt.Net.BCrypt.Verify(data.Password, user.PasswordHash))
			{
				var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretKeysecretKey"));
				var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

				var claims = new List<Claim> {
					new Claim(ClaimTypes.Name, user.Name),
					new Claim(ClaimTypes.NameIdentifier, user.Username),
					new Claim(ClaimTypes.Role, user.Role.ToString()),
				};

				var tokenOptions = new JwtSecurityToken(
					issuer: "https://localhost:7030", // FIXME
					audience: "https://localhost:7030", // FIXME
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