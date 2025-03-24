using System;
using System.Collections.Generic;

namespace DriveSmartAcademy.ViewModels
{
    public class CourseDetailViewModel
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Instructor { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public decimal ProgressPercentage { get; set; }
        public string Status { get; set; }
        public Guid? LastAccessedLessonId { get; set; }
        public List<LessonViewModel> Lessons { get; set; } = new List<LessonViewModel>();
    }

    public class LessonViewModel
    {
        public Guid LessonId { get; set; }
        public string Title { get; set; }
        public int OrderIndex { get; set; }
        public bool IsCompleted { get; set; }
    }
} 