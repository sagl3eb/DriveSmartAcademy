using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DriveSmartAcademy.Data;
using DriveSmartAcademy.Models;
using DriveSmartAcademy.ViewModels;

namespace DriveSmartAcademy.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }

        // User Management
        public IActionResult ManageUsers()
        {
            return View();
        }

        public IActionResult ApproveUsers()
        {
            return View();
        }

        // Learning Content
        public IActionResult ManageCourses()
        {
            return View();
        }

        public IActionResult ManageCategories()
        {
            return View();
        }

        // Exams & Quizzes
        public IActionResult ManageExams()
        {
            return View();
        }

        public IActionResult ConfigureTest()
        {
            return View();
        }

        // Student Performance
        public IActionResult MonitorPerformance()
        {
            return View();
        }

        public IActionResult GenerateReports()
        {
            return View();
        }

        // Communications
        public IActionResult Communications()
        {
            return View();
        }

        public IActionResult ManageAnnouncements()
        {
            return View();
        }

        // Error Handling
        [Route("Admin/Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            return View("Error");
        }
    }
}