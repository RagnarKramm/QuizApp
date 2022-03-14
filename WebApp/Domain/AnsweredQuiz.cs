using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain;

public class AnsweredQuiz : BaseEntity
{
    [MaxLength(64)]
    [Display(Name = "Answerer")]
    public string AnsweredBy { get; set; } = default!;
    
    public int? QuizQuestionsTotal { get; set; }
    public int? QuizQuestionsCorrect { get; set; }
    public int? PollQuestions { get; set; }

    public int QuizId { get; set; }
    public Quiz? Quiz { get; set; }

    public ICollection<QuizAnswer>? QuizAnswers { get; set; }
}