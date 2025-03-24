using System;
using System.Collections.Generic;

namespace DriveSmartAcademy.ViewModels
{
    public class ExamResultViewModel
    {
        public int Id { get; set; }
        public Guid QuizId { get; set; }
        public string QuizTitle { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public string TimeTaken { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string ResultClass { get; set; } = string.Empty;
        public DateTime CompletionDate { get; set; }
        public bool IsPassed { get; set; }
        public List<QuestionResultViewModel> Questions { get; set; } = new List<QuestionResultViewModel>();
    }

    public class QuestionResultViewModel
    {
        public string Question { get; set; } = string.Empty;
        public string UserAnswer { get; set; } = string.Empty;
        public string CorrectAnswer { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
} 