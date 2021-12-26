using System.Data.Common;
using System.Net;
using LmsPlatform.Data;
using LmsPlatform.Dtos;
using LmsPlatform.Models;
using LmsPlatform.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LmsPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
		private readonly DataContext _context;
		private readonly UserRepository _userRepository;

		public UsersController(DataContext context, UserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("")]
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
