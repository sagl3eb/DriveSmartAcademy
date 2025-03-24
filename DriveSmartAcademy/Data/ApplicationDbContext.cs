using Microsoft.EntityFrameworkCore;
using DriveSmartAcademy.Models;
using System;

namespace DriveSmartAcademy.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<CourseProgress> UserProgress { get; set; }
        public DbSet<CompletedLesson> CompletedLessons { get; set; }
        public DbSet<QuizResult> QuizResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<CourseProgress>()
                .HasOne(cp => cp.Course)
                .WithMany(c => c.CourseProgresses)
                .HasForeignKey(cp => cp.CourseID);

            modelBuilder.Entity<CourseProgress>()
                .HasOne(cp => cp.LastAccessedLesson)
                .WithMany(l => l.LastAccessedByProgresses)
                .HasForeignKey(cp => cp.LastAccessedLessonID);

            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.Course)
                .WithMany(c => c.Lessons)
                .HasForeignKey(l => l.CourseID);

            modelBuilder.Entity<User>()
                .HasKey(u => u.UserID);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .ToTable("Users", tb => tb.HasCheckConstraint(
                    "CK_Users_UserType",
                    "[UserType] IN ('admin', 'instructor', 'student')"
                ));
        }
    }
}
