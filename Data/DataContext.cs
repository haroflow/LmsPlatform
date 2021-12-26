﻿using LmsPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace LmsPlatform.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            //
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Module> Modules => Set<Module>();
        public DbSet<Lesson> Lessons => Set<Lesson>();
		public DbSet<LessonCompleted> LessonsCompleted => Set<LessonCompleted>();
	}
}
