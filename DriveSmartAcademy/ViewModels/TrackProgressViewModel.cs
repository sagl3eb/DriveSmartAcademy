using System;
using System.Collections.Generic;

namespace DriveSmartAcademy.ViewModels
{
    public class TrackProgressViewModel
    {
        public int EnrolledCourses { get; set; }
        public int CompletedCourses { get; set; }
        public int TotalCourses { get; set; }
        public decimal OverallProgressPercentage { get; set; }
        public int CompletedModules { get; set; }
        public int TotalModules { get; set; }
        public int CompletedQuizzes { get; set; }
        public int TotalQuizzes { get; set; } = 0; // Will be updated when quiz system is implemented
        public List<CourseProgressTrackViewModel> CourseProgressList { get; set; }
    }

    // CourseProgressViewModel for TrackProgress view
    public class CourseProgressTrackViewModel
    {
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public decimal ProgressPercentage { get; set; }
        public string Status { get; set; }
        public int CompletedModules { get; set; }
        public int TotalModules { get; set; }
        public string LastAccessedLesson { get; set; }
        public Guid? LastAccessedLessonId { get; set; }
    }
} 