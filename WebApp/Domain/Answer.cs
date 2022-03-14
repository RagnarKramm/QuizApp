using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain;

public class Answer : BaseEntity
{
    [Display(Name = "Correct answer")]
    public bool IsCorrect { get; set; } = default!;
    
    [MaxLength(256)]
    [Display(Name = "Answer")]
    public string AnswerText { get; set; } = default!;

    public int QuestionId { get; set; }
    public Question? Question { get; set; }
    
    public ICollection<QuizAnswer>? QuizAnswers { get; set; }
}