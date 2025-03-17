using System;
using System.Collections.Generic;

namespace DriveSmartAcademy.ViewModels
{
    public class StudentDashboardViewModel
    {
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public List<CourseViewModel> EnrolledCourses { get; set; }
        public List<QuizResultViewModel> RecentQuizResults { get; set; }
        public FeedbackViewModel StudentFeedback { get; set; }
        public List<ExamViewModel> DrivingTheoryExams { get; set; }
        public CourseProgressViewModel OverallProgress { get; set; }

        public StudentDashboardViewModel()
        {
            EnrolledCourses = new List<CourseViewModel>();
            RecentQuizResults = new List<QuizResultViewModel>();
            DrivingTheoryExams = new List<ExamViewModel>();
        }
    }

    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public int CompletionPercentage { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public List<ModuleViewModel> Modules { get; set; }

        public CourseViewModel()
        {
            Modules = new List<ModuleViewModel>();
        }
    }

    public class ModuleViewModel
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public bool IsCompleted { get; set; }
        public List<QuizViewModel> Quizzes { get; set; }

        public ModuleViewModel()
        {
            Quizzes = new List<QuizViewModel>();
        }
    }

    public class QuizViewModel
    {
        public int QuizId { get; set; }
        public string QuizName { get; set; }
        public bool IsCompleted { get; set; }
        public int? Score { get; set; }
    }

    public class QuizResultViewModel
    {
        public int QuizId { get; set; }
        public string QuizName { get; set; }
        public int Score { get; set; }
        public DateTime CompletionDate { get; set; }
        public string CourseName { get; set; }
    }

    public class FeedbackViewModel
    {
        public int FeedbackId { get; set; }
        public string FeedbackText { get; set; }
        public DateTime FeedbackDate { get; set; }
        public string InstructorResponse { get; set; }
        public DateTime? ResponseDate { get; set; }
    }

    public class ExamViewModel
    {
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public DateTime ExamDate { get; set; }
        public bool IsPassed { get; set; }
        public int? Score { get; set; }
    }

    public class CourseProgressViewModel
    {
        public int CompletedModules { get; set; }
        public int TotalModules { get; set; }
        public int CompletedQuizzes { get; set; }
        public int TotalQuizzes { get; set; }
        public double OverallPercentage { get; set; }
    }
}