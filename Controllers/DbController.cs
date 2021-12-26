using LmsPlatform.Data;
using LmsPlatform.Dtos;
using LmsPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using SlugGenerator;

namespace LmsPlatform.Controllers
{
    [ApiController]
    [Route("api/db")]
    public class DbController : Controller
    {
        private readonly DataContext _context;

        public DbController(DataContext context)
        {
            _context = context;
            Console.WriteLine("seed");
            this.Seed();
        }

        [HttpGet]
        [Route("teste")]
        public IActionResult Teste() {
            var c = new GetMyLessonDto();
            c.Slug = "slug";
            c.Title = "title";
            return Ok(c);
        }

        [HttpGet]
        [Route("seed")]
        public IActionResult Seed()
        {
            _context.Users.RemoveRange(_context.Users.ToList());
            _context.Courses.RemoveRange(_context.Courses.ToList());

            var defaultPassHash = BCrypt.Net.BCrypt.HashPassword("123");

            _context.Users.Add(new User()
            { Name = "Administrator Smith", Role = UserRole.Administrator, Username = "administrator", PasswordHash = defaultPassHash });
            _context.Users.Add(new User()
            { Name = "Teacher Mary", Role = UserRole.Teacher, Username = "teacher", PasswordHash = defaultPassHash });
            _context.Users.Add(new User()
            { Name = "Student John", Role = UserRole.Student, Username = "student", PasswordHash = defaultPassHash });

            _context.Courses.Add(new Course()
            {
                Name = "Angular 15",
                Description = "In this course you'll learn how to use Angular 15 to create amazing web applications",
                Slug = "Angular 15".GenerateSlug(),
                MiniatureImage = "https://www.jtinetwork.com/wp-content/uploads/2020/07/courseintroimage.jpg",
                BannerImage = "https://www.jtinetwork.com/wp-content/uploads/2020/07/courseintroimage.jpg",
                Modules = new List<Module> {
                    new Module() {
                        Title = "Introduction",
                        Slug = "Introduction".GenerateSlug(),
                        Lessons = new List<Lesson> {
                            new Lesson()
                            {
                                Title = "Introduction to Angular 15",
                                Slug = "Introduction to Angular 15".GenerateSlug(),
                                Text = "Welcome to my Angular 15 course! In this course you'll learn how to use Angular 15 to create amazing web applications",
                                VideoURL = "https://www.youtube.com/embed/_jrLiLjF0wE",
                            }
                        }
                    },
                    new Module() {
                        Title = "Getting to know Angular",
                        Slug = "Getting to know Angular".GenerateSlug(),
                        Lessons = new List<Lesson> {
                            new Lesson()
                            {
                                Title = "Creating a new sample project",
                                Slug = "Creating a new sample project".GenerateSlug(),
                                Text = "This is the first lesson. To create a project type `ng new my-sample-app` in your command prompt.",
                            },
                            new Lesson()
                            {
                                Title = "Creating a new component",
                                Slug = "Creating a new component".GenerateSlug(),
                                Text = "This is the second lesson. We'll create a new component using `ng generate`",
                                VideoURL = "https://www.youtube.com/embed/_jrLiLjF0wE",
                            },
                        }
                    }
                }
            });

            _context.Courses.Add(new Course()
            {
                Name = "C# 20",
                Description = "In this course you'll learn how to use C# 20 to create amazing applications",
                Slug = "C# 20".GenerateSlug(),
                MiniatureImage = "https://www.jtinetwork.com/wp-content/uploads/2020/07/courseintroimage.jpg",
                BannerImage = "https://www.jtinetwork.com/wp-content/uploads/2020/07/courseintroimage.jpg",
                Modules = new List<Module> {
                    new Module() {
                        Title = "Introduction",
                        Slug = "Introduction".GenerateSlug(),
                        Lessons = new List<Lesson> {
                            new Lesson()
                            {
                                Title = "Introduction to C# 20",
                                Slug = "Introduction to C# 20".GenerateSlug(),
                                Text = "Welcome to my C# 20 course! In this course you'll learn how to use C# 20 to create amazing applications",
                                VideoURL = "https://www.youtube.com/embed/_jrLiLjF0wE",
                            }
                        }
                    },
                    new Module() {
                        Title = "Getting to know C#",
                        Slug = "Getting to know C#".GenerateSlug(),
                        Lessons = new List<Lesson> {
                            new Lesson()
                            {
                                Title = "Creating a new sample project",
                                Slug = "Creating a new sample project".GenerateSlug(),
                                Text = "This is the first lesson. To create a project type `dotnet new webapi -o sample-web-api` in your command prompt.",
                                VideoURL = "https://www.youtube.com/embed/_jrLiLjF0wE",
                            },
                        }
                    }
                }
            });

            _context.SaveChanges();

            return Ok("ok");
        }
    }
}
