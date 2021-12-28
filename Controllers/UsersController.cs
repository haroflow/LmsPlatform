using LmsPlatform.Dtos;
using LmsPlatform.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LmsPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
		private readonly UserRepository _userRepository;

		public UsersController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var list = await _userRepository.GetAllUsersAsync();
            return Ok(list);
        }

        [HttpGet]
        [Route("check-username-availability")]
        public async Task<IActionResult> CheckUsernameAvailability([FromQuery] CheckUsernameAvailabilityRequest data)
        {
            var available = await _userRepository
                .IsUsernameAvailableAsync(data.Username);

            return Ok(new CheckUsernameAvailabilityResponse {
                Available = available
            });
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudentRequest data)
        {
            try
            {
                var user = await _userRepository.RegisterStudentAsync(data);
                return Ok();
            }
            catch (Exception)
            {
                // TODO return custom message if username already exists.
                // Check if the exception using SQL Server returns any indication of a duplicate key error.
                return Problem("Could not register user");
            }
        }

    }
}
