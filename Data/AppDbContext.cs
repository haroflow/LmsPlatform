using LmsPlatform.Models;
using Microsoft.EntityFrameworkCore;
using SlugGenerator;

namespace LmsPlatform.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
            //
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Module> Modules => Set<Module>();
        public DbSet<Lesson> Lessons => Set<Lesson>();
		public DbSet<LessonCompleted> LessonCompleted => Set<LessonCompleted>();

        internal void Seed()
        {
            Console.WriteLine("seeding database...");

            Users.RemoveRange(Users.ToList());
            LessonCompleted.RemoveRange(LessonCompleted.ToList());
            Lessons.RemoveRange(Lessons.ToList());
            Modules.RemoveRange(Modules.ToList());
            Courses.RemoveRange(Courses.ToList());

            var defaultPassHash = BCrypt.Net.BCrypt.HashPassword("123");

            Users.Add(new User()
            { Name = "Administrator Smith", Role = UserRole.Administrator, Username = "administrator", PasswordHash = defaultPassHash });
            Users.Add(new User()
            { Name = "Teacher Mary", Role = UserRole.Teacher, Username = "teacher", PasswordHash = defaultPassHash });
            Users.Add(new User()
            { Name = "Student John", Role = UserRole.Student, Username = "student", PasswordHash = defaultPassHash });

            Courses.Add(new Course()
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

            Courses.Add(new Course()
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

            SaveChanges();
        }
    }
}
