using LmsPlatform.Data;
using LmsPlatform.Dtos;
using LmsPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace LmsPlatform.Repositories
{
	public class UserRepository
	{
		private readonly DataContext _context;

		public UserRepository(DataContext context)
		{
			_context = context;
		}

		public async Task<bool> IsUsernameAvailableAsync(string username) {
			var inUse = await _context.Users
				.AnyAsync(u => u.Username == username);

			return !inUse;
		}

		public async Task<IEnumerable<User>> GetAllUsersAsync() {
			var list = await _context.Users.ToListAsync();
			return list;
		}

		public async Task<User> RegisterStudentAsync(RegisterStudentRequest data) {
			var passwordHash = BCrypt.Net.BCrypt.HashPassword(data.Password);
			var user = new User() {
				Username = data.Username,
				PasswordHash = passwordHash,
				Role = UserRole.Student,
			};

			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			return user;
		}

		public async Task<List<Course>?> GetUserEnrolledCoursesAsync(string? username)
		{
			var user = (await _context.Users
                .Include(u => u.MyCourses)
                .FirstOrDefaultAsync(u => u.Username == username));
			if (user == null)
				return null;
			
			return user.MyCourses;
		}

		public async Task<User?> GetUser(string? username)
		{
			return await _context.Users.FindAsync(username);
		}
	}
}