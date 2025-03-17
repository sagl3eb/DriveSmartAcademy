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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
