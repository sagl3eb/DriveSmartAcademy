using System.ComponentModel.DataAnnotations;
using System;

namespace DriveSmartAcademy.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password must be between {2} and {1} characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")] // Matches NVARCHAR(50)
        [RegularExpression(@"^[a-zA-Z\s-']{1,50}$", ErrorMessage = "First name can only contain letters, spaces, hyphens and apostrophes")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")] // Matches NVARCHAR(50)
        [RegularExpression(@"^[a-zA-Z\s-']{1,50}$", ErrorMessage = "Last name can only contain letters, spaces, hyphens and apostrophes")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [Range(typeof(DateTime), "1900-01-01", "2100-01-01", ErrorMessage = "Please enter a valid date")]
        public DateTime DateOfBirth { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")] // Matches NVARCHAR(20)
        [RegularExpression(@"^\+?[\d\s-()]{10,20}$", ErrorMessage = "Please enter a valid phone number")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        //[StringLength(int.MaxValue, ErrorMessage = "Address is too long")] // Matches NVARCHAR(MAX)
        //public string? Address { get; set; }

        //[Required(ErrorMessage = "User type is required")]
        //[StringLength(20, ErrorMessage = "User type cannot exceed 20 characters")] // Matches NVARCHAR(20)
        //[RegularExpression(@"^(admin|instructor|student)$", ErrorMessage = "Invalid user type")]
        //[Display(Name = "User Type")]
        //public string UserType { get; set; }

        [Required(ErrorMessage = "Street address is required")]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "Region is required")]
        public string Region { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }
    }
}
