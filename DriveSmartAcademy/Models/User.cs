using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DriveSmartAcademy.Models
{
    public class User
    {
        public Guid UserID { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? StreetAddress { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? UserType { get; set; }  // admin, instructor, or student
        public string? ProfilePictureUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool? IsActive { get; set; }
    }
}
