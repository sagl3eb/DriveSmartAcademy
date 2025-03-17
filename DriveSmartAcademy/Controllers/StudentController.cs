using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DriveSmartAcademy.Models;
using DriveSmartAcademy.ViewModels;
using DriveSmartAcademy.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace DriveSmartAcademy.Controllers
{
    [Authorize(Policy = "RequireStudentRole")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirstValue("UserId");
            var user = await _context.Users.FindAsync(Guid.Parse(userId));

            if (user == null)
                return NotFound();

            // Create ViewModel with dummy data for now
            // In a real application, this would fetch from the database
            var viewModel = new StudentDashboardViewModel
            {
                StudentName = $"{user.FirstName} {user.LastName}",
                StudentEmail = user.Email
                // Other properties would be populated from the database
            };

            return View(viewModel);
        }

        public IActionResult ViewCourses()
        {
            return View();
        }

        public IActionResult SelectCourse(int id)
        {
            return View();
        }

        public IActionResult CompleteCourse(int id)
        {
            return View();
        }

        public IActionResult AttemptQuiz(int id)
        {
            return View();
        }

        public IActionResult ViewQuizResults(int id)
        {
            return View();
        }

        public IActionResult TrackProgress()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GiveFeedback()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GiveFeedback(FeedbackViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Save feedback logic here

            return RedirectToAction("Dashboard");
        }

        public IActionResult TakeDrivingTheoryExam()
        {
            return View();
        }
    }
}