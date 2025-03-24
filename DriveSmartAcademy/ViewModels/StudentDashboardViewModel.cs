using System;
using System.Collections.Generic;

namespace DriveSmartAcademy.ViewModels
{
    public class StudentDashboardViewModel
    {
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public ICollection<CourseViewModel> EnrolledCourses { get; set; }
        public ICollection<QuizResultViewModel> RecentQuizResults { get; set; }
        public ICollection<ExamViewModel> DrivingTheoryExams { get; set; }
        public CourseProgressViewModel OverallProgress { get; set; }
        public FeedbackViewModel StudentFeedback { get; set; }
    }

    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public decimal CompletionPercentage { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public ICollection<ModuleViewModel> Modules { get; set; }
    }

    public class ModuleViewModel
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class QuizResultViewModel
    {
        public int QuizId { get; set; }
        public string QuizName { get; set; }
        public int Score { get; set; }
        public DateTime CompletionDate { get; set; }
        public string CourseName { get; set; }
    }

    public class ExamViewModel
    {
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public DateTime ExamDate { get; set; }
        public bool IsPassed { get; set; }
    }

    public class CourseProgressViewModel
    {
        public decimal OverallPercentage { get; set; }
        public int CompletedModules { get; set; }
        public int TotalModules { get; set; }
        public int CompletedQuizzes { get; set; }
        public int TotalQuizzes { get; set; }
    }
}