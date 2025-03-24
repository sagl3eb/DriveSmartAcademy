using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DriveSmartAcademy.Models;
using DriveSmartAcademy.ViewModels;
using DriveSmartAcademy.Data;
using DriveSmartAcademy.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace DriveSmartAcademy.Controllers
{
    // Temporarily comment out the authorization policy to allow access for testing
    // [Authorize(Policy = "RequireStudentRole")]
    // [Authorize] // Use basic authorization instead
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CourseProgressService _progressService;
        // Store courses in a static variable for demo purposes
        // In a real application, this would be stored in the database
        private static List<CourseViewModel> _courses;

        public StudentController(ApplicationDbContext context, CourseProgressService progressService)
        {
            _context = context;
            _progressService = progressService;
            
            // Initialize sample courses if not already initialized
            if (_courses == null)
            {
                _courses = new List<CourseViewModel>
                {
                    new CourseViewModel
                    {
                        CourseId = 1,
                        CourseName = "Road Rules and Regulations",
                        Description = "Learn essential traffic rules, road signs, and regulations for safe driving.",
                        CompletionPercentage = 75,
                        EnrollmentDate = DateTime.Now.AddDays(-30),
                        Modules = new List<ModuleViewModel>
                        {
                            new ModuleViewModel { ModuleId = 1, ModuleName = "Traffic Signs and Signals", IsCompleted = true },
                            new ModuleViewModel { ModuleId = 2, ModuleName = "Right of Way Rules", IsCompleted = true },
                            new ModuleViewModel { ModuleId = 3, ModuleName = "Speed Limits and Safe Driving", IsCompleted = true },
                            new ModuleViewModel { ModuleId = 4, ModuleName = "Parking Regulations", IsCompleted = false }
                        }
                    },
                    new CourseViewModel
                    {
                        CourseId = 2,
                        CourseName = "Defensive Driving Techniques",
                        Description = "Master defensive driving strategies to anticipate hazards and prevent accidents.",
                        CompletionPercentage = 45,
                        EnrollmentDate = DateTime.Now.AddDays(-20),
                        Modules = new List<ModuleViewModel>
                        {
                            new ModuleViewModel { ModuleId = 5, ModuleName = "Hazard Awareness", IsCompleted = true },
                            new ModuleViewModel { ModuleId = 6, ModuleName = "Space Management", IsCompleted = true },
                            new ModuleViewModel { ModuleId = 7, ModuleName = "Crash Avoidance Techniques", IsCompleted = false },
                            new ModuleViewModel { ModuleId = 8, ModuleName = "Adverse Weather Driving", IsCompleted = false }
                        }
                    },
                    new CourseViewModel
                    {
                        CourseId = 3,
                        CourseName = "Vehicle Safety and Maintenance",
                        Description = "Understand vehicle safety features and basic maintenance procedures.",
                        CompletionPercentage = 10,
                        EnrollmentDate = DateTime.Now.AddDays(-10),
                        Modules = new List<ModuleViewModel>
                        {
                            new ModuleViewModel { ModuleId = 9, ModuleName = "Vehicle Safety Systems", IsCompleted = true },
                            new ModuleViewModel { ModuleId = 10, ModuleName = "Basic Maintenance Checks", IsCompleted = false },
                            new ModuleViewModel { ModuleId = 11, ModuleName = "Tire Safety and Maintenance", IsCompleted = false },
                            new ModuleViewModel { ModuleId = 12, ModuleName = "Emergency Repairs", IsCompleted = false }
                        }
                    }
                };
            }
        }

        public async Task<IActionResult> Dashboard()
        {
            User user = null;
            
            try 
        {
            var userId = User.FindFirstValue("UserId");
                
                // If userId is not found, check for NameIdentifier claim (standard claim for user ID)
                if (string.IsNullOrEmpty(userId))
                {
                    userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                }
                
                if (!string.IsNullOrEmpty(userId))
                {
                    user = await _context.Users.FindAsync(Guid.Parse(userId));
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error retrieving user: {ex.Message}");
            }

            // If no user is found, create a temporary one for display purposes
            if (user == null)
            {
                // Create a temporary user object for demo purposes
                user = new User
                {
                    UserID = Guid.NewGuid(),
                    FirstName = "Demo",
                    LastName = "Student",
                    Email = "demo.student@example.com"
                };
            }

            // Get real progress data if user exists
            var overallProgress = new CourseProgressViewModel
            {
                OverallPercentage = 0,
                CompletedModules = 0,
                TotalModules = 0,
                CompletedQuizzes = 0,
                TotalQuizzes = 0
            };

            var enrolledCourses = _courses;
            
            try
            {
                // Get overall progress if user is authenticated
                if (user.UserID != Guid.Empty)
                {
                    var progressData = await _progressService.GetOverallProgress(user.UserID);
                    
                    // Convert to dashboard format
                    overallProgress.OverallPercentage = progressData.OverallProgressPercentage;
                    overallProgress.CompletedModules = progressData.CompletedModules;
                    overallProgress.TotalModules = progressData.TotalModules;
                    overallProgress.CompletedQuizzes = progressData.CompletedQuizzes;
                    overallProgress.TotalQuizzes = progressData.TotalQuizzes;
                    
                    // If there are real course progress entries, use them instead of sample data
                    if (progressData.CourseProgressList.Any())
                    {
                        // We'll still use sample courses for now
                        // In a real implementation, you would convert the real course progress to CourseViewModel
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error but continue with demo data
                Console.WriteLine($"Error getting progress data: {ex.Message}");
            }

            // Sample feedback for demo purposes
            var sampleFeedback = new FeedbackViewModel
            {
                FeedbackId = 1,
                FeedbackText = "The courses are well structured and easy to follow. I especially enjoyed the interactive quizzes.",
                FeedbackDate = DateTime.Now.AddDays(-5),
                Rating = 5,
                CourseTitle = "Road Rules and Regulations",
                InstructorName = "John Smith"
            };

            // Create ViewModel with sample data
            var viewModel = new StudentDashboardViewModel
            {
                StudentName = $"{user.FirstName} {user.LastName}",
                StudentEmail = user.Email,
                EnrolledCourses = enrolledCourses,
                RecentQuizResults = new List<QuizResultViewModel>
                {
                    new QuizResultViewModel
                    {
                        QuizId = 1,
                        QuizName = "Road Signs Quiz",
                        Score = 85,
                        CompletionDate = DateTime.Now.AddDays(-5),
                        CourseName = "Road Rules and Regulations"
                    },
                    new QuizResultViewModel
                    {
                        QuizId = 2,
                        QuizName = "Right of Way Quiz",
                        Score = 92,
                        CompletionDate = DateTime.Now.AddDays(-3),
                        CourseName = "Road Rules and Regulations"
                    },
                    new QuizResultViewModel
                    {
                        QuizId = 3,
                        QuizName = "Hazard Perception Quiz",
                        Score = 78,
                        CompletionDate = DateTime.Now.AddDays(-1),
                        CourseName = "Defensive Driving Techniques"
                    }
                },
                DrivingTheoryExams = new List<ExamViewModel>
                {
                    new ExamViewModel
                    {
                        ExamId = 1,
                        ExamName = "Official Driving Theory Test",
                        ExamDate = DateTime.Now.AddDays(14),
                        IsPassed = false
                    }
                },
                OverallProgress = overallProgress,
                StudentFeedback = sampleFeedback
            };

            return View(viewModel);
        }

        public IActionResult MyCourses()
        {
            ViewData["HideDashboardSummary"] = true;
            return View(_courses.Where(c => c.CompletionPercentage > 0).ToList());
        }

        public IActionResult ViewCourses()
        {
            ViewData["HideDashboardSummary"] = true;
            return View(_courses);
        }

        public IActionResult SelectCourse(int? id, Guid? courseId)
        {
            // Handle both int and Guid course IDs
            CourseViewModel course = null;
            
            if (id.HasValue)
            {
                // Find by integer ID (for demo courses)
                course = _courses.FirstOrDefault(c => c.CourseId == id.Value);
            }
            else if (courseId.HasValue)
            {
                // Find by Guid ID (for database courses)
                // First check if we have a matching demo course by name
                var databaseCourse = _context.Courses.FirstOrDefault(c => c.CourseID == courseId.Value);
                if (databaseCourse != null)
                {
                    // Try to find a matching demo course
                    var matchingCourse = _courses.FirstOrDefault(c => 
                        c.CourseName.Equals(databaseCourse.Title, StringComparison.OrdinalIgnoreCase));
                    
                    if (matchingCourse != null)
                    {
                        course = matchingCourse;
                    }
                    else
                    {
                        // Create a new view model from the database course
                        course = new CourseViewModel
                        {
                            CourseId = 0, // Use a placeholder ID
                            CourseName = databaseCourse.Title,
                            Description = databaseCourse.Description,
                            CompletionPercentage = 0, // Will be calculated later
                            EnrollmentDate = DateTime.Now,
                            Modules = new List<ModuleViewModel>() // Populate with actual modules if available
                        };
                        
                        // TODO: Add actual modules from the database if available
                    }
                }
            }
            
            if (course == null)
                return NotFound();
                
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteModule(int? courseId, Guid? courseGuid, string moduleId)
        {
            CourseViewModel course = null;
            
            if (courseId.HasValue)
            {
                // Find by integer ID (for demo courses)
                course = _courses.FirstOrDefault(c => c.CourseId == courseId.Value);
            }
            else if (courseGuid.HasValue)
            {
                // Find by Guid ID (from database) - first check if there's a matching demo course
                var databaseCourse = await _context.Courses.FirstOrDefaultAsync(c => c.CourseID == courseGuid.Value);
                if (databaseCourse != null)
                {
                    // Try to find a demo course with matching name
                    course = _courses.FirstOrDefault(c => 
                        c.CourseName.Equals(databaseCourse.Title, StringComparison.OrdinalIgnoreCase));
                }
            }
            
            if (course == null)
                return NotFound();
                
            // Try to find the module by its ID, supporting both int and Guid IDs
            var module = course.Modules.FirstOrDefault(m => m.ModuleId.ToString() == moduleId);
            if (module == null)
                return NotFound();
                
            // Mark the module as completed
            module.IsCompleted = true;
            
            // Update course completion percentage
            UpdateCourseProgress(course);
            
            // If this is a database course, also update it in the database
            if (courseGuid.HasValue)
            {
                try
                {
                    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (!string.IsNullOrEmpty(userId))
                    {
                        // For database interactions, attempt to convert moduleId to a Guid
                        Guid moduleGuid;
                        if (Guid.TryParse(moduleId, out moduleGuid))
                        {
                            var moduleEntity = await _context.Lessons
                                .FirstOrDefaultAsync(l => l.LessonID == moduleGuid);
                                
                            if (moduleEntity != null)
                            {
                                // Record completion in the database
                                await _progressService.UpdateProgress(
                                    Guid.Parse(userId), 
                                    courseGuid.Value, 
                                    moduleEntity.LessonID);
                            }
                        }
                        else 
                        {
                            // If it's not a Guid, try to find it by integer ID
                            int moduleIntId;
                            if (int.TryParse(moduleId, out moduleIntId))
                            {
                                var moduleEntity = await _context.Lessons
                                    .FirstOrDefaultAsync(l => l.OrderIndex == moduleIntId);
                                    
                                if (moduleEntity != null)
                                {
                                    // Record completion in the database
                                    await _progressService.UpdateProgress(
                                        Guid.Parse(userId), 
                                        courseGuid.Value, 
                                        moduleEntity.LessonID);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log error but don't fail the action
                    Console.WriteLine($"Error updating database: {ex.Message}");
                }
            }
            
            // Redirect to course page
            if (courseId.HasValue)
                return RedirectToAction("SelectCourse", new { id = courseId.Value });
            else
                return RedirectToAction("SelectCourse", new { courseId = courseGuid });
        }

        public IActionResult CourseDetails(int id)
        {
            ViewData["HideDashboardSummary"] = true;
            var course = _courses.FirstOrDefault(c => c.CourseId == id);
            if (course == null)
                return NotFound();
                
            return View(course);
        }

        public IActionResult AttemptQuiz(int id)
        {
            return View();
        }

        public async Task<IActionResult> ViewQuizResults()
        {
            ViewData["HideDashboardSummary"] = true;
            try
            {
                // Get current user ID - use guest-user if not authenticated
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    userId = "guest-user";
                }
                
                // Debug output
                Console.WriteLine($"Looking for quiz results for user ID: {userId}");
                
                // Fetch quiz results for this user from database
                var results = await _context.QuizResults
                    .Where(r => r.UserId == userId)
                    .OrderByDescending(r => r.CompletionDate)
                    .ToListAsync();

                // Debug output
                Console.WriteLine($"Found {results.Count} results for user {userId}");
                
                // Create view models from the database results
                var viewModels = results.Select(r => {
                    Console.WriteLine($"Processing result ID: {r.Id}, Score: {r.Score}");
                    
                    // Since QuizTitle isn't in the database, all theory exams will show with a default title
                    return new ExamResultViewModel
                    {
                        Id = r.Id,
                        QuizId = Guid.NewGuid(), // Generate a new ID since it's not in the database
                        QuizTitle = "Theory Exam",
                        Score = r.Score,
                        TotalQuestions = 20, // Default since it's not stored
                        TimeTaken = r.TimeTaken,
                        CompletionDate = r.CompletionDate,
                        IsPassed = r.IsPassed,
                        // Set Date for display purposes
                        Date = r.CompletionDate.ToString("MMM dd, yyyy"),
                        // Set Result for display purposes
                        Result = r.IsPassed ? "Passed" : (r.Score >= 70 ? "Almost" : "Failed"),
                        // Set ResultClass for styling
                        ResultClass = r.IsPassed ? "bg-success" : (r.Score >= 70 ? "bg-warning text-dark" : "bg-danger"),
                        // Set all results as Theory exams
                        Type = "Theory"
                    };
                }).ToList();

                // Debug output
                Console.WriteLine($"Prepared {viewModels.Count} view models for display");
                
                // Add a message for guest users
                if (userId == "guest-user")
                {
                    ViewData["GuestUserMessage"] = "You are viewing results as a guest. To save your results permanently, please create an account.";
                }
                
                return View(viewModels);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error retrieving quiz results: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Return empty list with message
                ViewData["ErrorMessage"] = "Unable to retrieve quiz results. Please try again later.";
                return View(new List<ExamResultViewModel>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveQuizResult(QuizResult quizResult)
        {
            try
            {
                // Log the received data for debugging
                Console.WriteLine($"Received quiz result: QuizTitle={quizResult.QuizTitle}, Score={quizResult.Score}");

                // Get the current user ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    Console.WriteLine("Unauthorized: User ID not found in claims");
                    return Unauthorized();
                }

                // Set the user ID and any missing fields
                quizResult.UserId = userId;
                quizResult.QuizId = quizResult.QuizId != Guid.Empty ? quizResult.QuizId : Guid.NewGuid();
                
                // Ensure we have a valid completion date
                if (quizResult.CompletionDate == default)
                {
                    quizResult.CompletionDate = DateTime.Now;
                }
                
                // Set IsPassed based on score (usually 70% is passing for quizzes)
                quizResult.IsPassed = quizResult.Score >= 70;

                // Add to the database
                await _context.QuizResults.AddAsync(quizResult);
                await _context.SaveChangesAsync();
                
                Console.WriteLine($"Quiz result saved successfully for user {userId}");
                
                // Check if it's an AJAX request
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Ok(new { success = true, message = "Quiz result saved successfully" });
                }
                else
                {
                    return RedirectToAction("ViewQuizResults");
                }
            }
            catch (Exception ex)
            {
                // Log the full exception details
                Console.WriteLine($"Error saving quiz result: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                
                // Check if it's an AJAX request
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    // Return a detailed error response
                    return StatusCode(500, new { 
                        success = false, 
                        message = "Failed to save quiz result", 
                        error = ex.Message 
                    });
                }
                else
                {
                    // Add error message to TempData
                    TempData["ErrorMessage"] = $"Failed to save quiz result: {ex.Message}";
                    return RedirectToAction("ViewQuizResults");
                }
            }
        }
        
        // Form-based version for traditional form submissions
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveQuizResultForm(QuizResult quizResult)
        {
            try
            {
                // Log the received form data
                Console.WriteLine($"Received form quiz result: QuizTitle={quizResult.QuizTitle}, Score={quizResult.Score}");

                // Get the current user ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "User not authenticated";
                    return RedirectToAction("Dashboard");
                }

                // Set the user ID and any missing fields
                quizResult.UserId = userId;
                quizResult.QuizId = Guid.NewGuid();
                quizResult.CompletionDate = DateTime.Now;
                
                // Set IsPassed based on score (usually 70% is passing for quizzes)
                quizResult.IsPassed = quizResult.Score >= 70;

                // Add to the database
                await _context.QuizResults.AddAsync(quizResult);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Form quiz result saved successfully for user {userId}");
                return RedirectToAction("ViewQuizResults");
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error saving form quiz result: {ex.Message}");
                
                // Add error message to TempData
                TempData["ErrorMessage"] = $"Failed to save quiz result: {ex.Message}";
                return RedirectToAction("ViewQuizResults");
            }
        }

        [HttpPost]
        public IActionResult ViewQuizResults(string examResultData)
        {
            // Pass the exam result data to the view using TempData
            if (!string.IsNullOrEmpty(examResultData))
            {
                TempData["ExamResultData"] = examResultData;
            }
            
            return View();
        }

        public async Task<IActionResult> TrackProgress()
        {
            ViewData["HideDashboardSummary"] = true;
            try
            {
                // Get the current user's ID
                string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                
                // Fallback to other possible claim types
                if (string.IsNullOrEmpty(userIdString))
                {
                    userIdString = User.FindFirstValue("UserId");
                }

                // Create demo progress data if user ID not found
                if (string.IsNullOrEmpty(userIdString))
                {
                    // Create a demo TrackProgressViewModel
                    var demoProgress = new TrackProgressViewModel
                    {
                        EnrolledCourses = 3,
                        CompletedCourses = 1,
                        TotalCourses = 5,
                        OverallProgressPercentage = 45,
                        CompletedModules = 9,
                        TotalModules = 20,
                        CompletedQuizzes = 5,
                        TotalQuizzes = 10,
                        CourseProgressList = _courses.Select(c => new CourseProgressTrackViewModel
                        {
                            CourseId = Guid.NewGuid(),
                            CourseName = c.CourseName,
                            EnrollmentDate = c.EnrollmentDate,
                            CompletionDate = c.CompletionPercentage == 100 ? c.EnrollmentDate.AddDays(30) : null,
                            ProgressPercentage = c.CompletionPercentage,
                            Status = c.CompletionPercentage == 100 ? "Completed" : "In Progress",
                            CompletedModules = c.Modules?.Count(m => m.IsCompleted) ?? 0,
                            TotalModules = c.Modules?.Count ?? 0,
                            LastAccessedLesson = c.Modules?.FirstOrDefault(m => !m.IsCompleted)?.ModuleName ?? c.Modules?.LastOrDefault()?.ModuleName,
                            LastAccessedLessonId = null
                        }).ToList()
                    };
                    
                    return View(demoProgress);
                }

                Guid userId;
                if (!Guid.TryParse(userIdString, out userId))
                {
                    // If user ID is not a valid GUID, create a deterministic GUID based on the string
                    userId = new Guid(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(userIdString)));
                }
                
                var progress = await _progressService.GetOverallProgress(userId);
                
                return View(progress);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in TrackProgress: {ex.Message}");
                
                // Return a friendly error view with demo data
                var demoProgress = new TrackProgressViewModel
                {
                    EnrolledCourses = 3,
                    CompletedCourses = 1,
                    TotalCourses = 5,
                    OverallProgressPercentage = 45,
                    CompletedModules = 9,
                    TotalModules = 20,
                    CompletedQuizzes = 5,
                    TotalQuizzes = 10,
                    CourseProgressList = _courses.Select(c => new CourseProgressTrackViewModel
                    {
                        CourseId = Guid.NewGuid(),
                        CourseName = c.CourseName,
                        EnrollmentDate = c.EnrollmentDate,
                        CompletionDate = c.CompletionPercentage == 100 ? c.EnrollmentDate.AddDays(30) : null,
                        ProgressPercentage = c.CompletionPercentage,
                        Status = c.CompletionPercentage == 100 ? "Completed" : "In Progress",
                        CompletedModules = c.Modules?.Count(m => m.IsCompleted) ?? 0,
                        TotalModules = c.Modules?.Count ?? 0,
                        LastAccessedLesson = c.Modules?.FirstOrDefault(m => !m.IsCompleted)?.ModuleName ?? c.Modules?.LastOrDefault()?.ModuleName,
                        LastAccessedLessonId = null
                    }).ToList()
                };
                
                return View(demoProgress);
            }
        }

        [HttpGet]
        public IActionResult GiveFeedback()
        {
            ViewData["HideDashboardSummary"] = true;
            var model = new FeedbackViewModel
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                FeedbackDate = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GiveFeedback(FeedbackViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Set feedback date to current time
            model.FeedbackDate = DateTime.Now;

            // In a real implementation, you would save to database here
            // For example:
            // await _context.Feedback.AddAsync(new Feedback { ... });
            // await _context.SaveChangesAsync();
            
            // Simulate async operation
            await Task.Delay(10);

            // Show success message
            TempData["SuccessMessage"] = "Thank you for your feedback! We appreciate your input.";

            return RedirectToAction(nameof(GiveFeedback));
        }

        public IActionResult TakeDrivingTheoryExam()
        {
            ViewData["HideDashboardSummary"] = true;
            // Return the view directly instead of redirecting
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitTheoryExam(QuizResult examResult)
        {
            try
            {
                // Log the received data
                Console.WriteLine($"Received theory exam result: Score={examResult.Score}");

                // Get the current user ID but don't require authentication
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Console.WriteLine($"Current user ID: {userId ?? "null"}");
                
                // If user is not authenticated, use a default user ID
                if (string.IsNullOrEmpty(userId))
                {
                    userId = "guest-user";
                    Console.WriteLine("Using default guest user ID for unauthenticated user");
                }

                // Set core required fields from the database schema
                examResult.UserId = userId;
                
                // Make sure required fields are set
                if (string.IsNullOrEmpty(examResult.TimeTaken))
                {
                    examResult.TimeTaken = "10:00";
                    Console.WriteLine("Set default TimeTaken to 10:00");
                }
                
                // Make sure date is set
                examResult.CompletionDate = DateTime.Now;
                Console.WriteLine($"Set completion date to: {examResult.CompletionDate}");

                // Explicitly create a clean entity with only the fields in the database
                var dbEntity = new QuizResult
                {
                    UserId = examResult.UserId,
                    Score = examResult.Score,
                    TimeTaken = examResult.TimeTaken,
                    CompletionDate = examResult.CompletionDate,
                    IsPassed = examResult.IsPassed
                };

                // Add to the database
                Console.WriteLine("Attempting to save to database...");
                await _context.QuizResults.AddAsync(dbEntity);
                await _context.SaveChangesAsync();
                
                Console.WriteLine($"Theory exam result saved successfully. ID: {dbEntity.Id}, User: {userId}");
                
                // Add success message to TempData
                TempData["SuccessMessage"] = "Theory exam results saved successfully!";
                
                // Redirect to the quiz results page
                return RedirectToAction("ViewQuizResults");
            }
            catch (Exception ex)
            {
                // Log the error in detail
                Console.WriteLine($"Error in SubmitTheoryExam: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "There was an error saving your exam results. Please try again or contact support.";
                
                // Return to the theory exams page
                return RedirectToAction("TakeDrivingTheoryExam");
            }
        }

        public async Task<IActionResult> ResumeLesson(Guid lessonId)
        {
            try
            {
                // Get the lesson details
                var lesson = await _context.Lessons
                    .Include(l => l.Course)
                    .FirstOrDefaultAsync(l => l.LessonID == lessonId);
                    
                if (lesson == null)
                {
                    return NotFound("Lesson not found");
                }
                
                // Redirect to the course with the specific lesson
                return RedirectToAction("SelectCourse", new { id = lesson.CourseID });
                
                // In a real implementation, you would redirect to a specific lesson view:
                // return RedirectToAction("ViewLesson", new { courseId = lesson.CourseID, lessonId = lesson.LessonID });
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in ResumeLesson: {ex.Message}");
                
                // Return a friendly error view
                return View("Error", new DriveSmartAcademy.ViewModels.ErrorViewModel { 
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Message = $"Error in ResumeLesson: {ex.Message}"
                });
            }
        }

        // Helper methods
        private CourseProgressViewModel CalculateOverallProgress()
        {
            int totalModules = 0;
            int completedModules = 0;
            
            foreach (var course in _courses)
            {
                if (course.Modules != null && course.Modules.Any())
                {
                    totalModules += course.Modules.Count;
                    completedModules += course.Modules.Count(m => m.IsCompleted);
                }
            }
            
            // Calculate overall percentage and convert to decimal
            decimal overallPercentage = totalModules > 0 
                ? (decimal)Math.Round((double)completedModules / totalModules * 100, 0) 
                : 0;
                
            return new CourseProgressViewModel
            {
                CompletedModules = completedModules,
                TotalModules = totalModules,
                CompletedQuizzes = 3,
                TotalQuizzes = 8,
                OverallPercentage = overallPercentage
            };
        }
        
        private void UpdateCourseProgress(CourseViewModel course)
        {
            if (course.Modules != null && course.Modules.Any())
            {
                int totalModules = course.Modules.Count;
                int completedModules = course.Modules.Count(m => m.IsCompleted);
                
                course.CompletionPercentage = (decimal)Math.Round((double)completedModules / totalModules * 100, 0);
            }
        }

        // Helper method to get a stable hash code from a string
        private int GetStableHashCode(string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;
            
            unchecked
            {
                int hash = 23;
                foreach (char c in str)
                {
                    hash = hash * 31 + c;
                }
                return hash;
            }
        }

        // Add Courses action to match the navigation menu
        public IActionResult Courses()
        {
            ViewData["HideDashboardCards"] = true;
            return View(_courses);
        }

        // Add TheoryExams action to match the navigation menu
        public IActionResult TheoryExams()
        {
            ViewData["HideDashboardCards"] = true;
            
            // Check if user is authenticated
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                Console.WriteLine("User is not authenticated when accessing TheoryExams");
                // Don't redirect, but set a warning message
                ViewData["AuthWarning"] = "You are not logged in. Your exam results will not be saved unless you log in.";
            }
            else
            {
                Console.WriteLine($"User {User.FindFirstValue(ClaimTypes.NameIdentifier)} is accessing TheoryExams");
            }
            
            return View();
        }

        // Redirects from the old QuizResults endpoint to the new ViewQuizResults endpoint
        public IActionResult QuizResults()
        {
            return RedirectToAction("ViewQuizResults");
        }

        // Method to check authentication status for client-side
        [HttpGet]
        public IActionResult CheckAuth()
        {
            bool isAuthenticated = User.Identity?.IsAuthenticated ?? false;
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            
            Console.WriteLine($"CheckAuth called. IsAuthenticated: {isAuthenticated}, UserId: {userId}");
            
            return Json(new { 
                isAuthenticated = isAuthenticated,
                userId = userId
            });
        }

        // Debug action to display raw quiz results data
        public async Task<IActionResult> DebugQuizResults()
        {
            try
            {
                var results = await _context.QuizResults.ToListAsync();
                var data = new
                {
                    Count = results.Count,
                    Results = results.Select(r => new
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        Score = r.Score,
                        TimeTaken = r.TimeTaken,
                        CompletionDate = r.CompletionDate,
                        IsPassed = r.IsPassed
                    }).OrderByDescending(r => r.CompletionDate).Take(10).ToList()
                };
                
                return Json(data);
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }
        }
    }
}