using System;

namespace DriveSmartAcademy.ViewModels
{
    public class FeedbackViewModel
    {
        public int FeedbackId { get; set; }
        public string? UserId { get; set; }
        public string FeedbackText { get; set; } = string.Empty;
        public DateTime FeedbackDate { get; set; } = DateTime.Now;
        public string? InstructorResponse { get; set; }
        public DateTime? ResponseDate { get; set; }
        public int Rating { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
    }
} 