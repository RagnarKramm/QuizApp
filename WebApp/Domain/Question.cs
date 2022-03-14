using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain;

public class Question : BaseEntity
{
    [MaxLength(1024)]
    [Display(Name = "Question")]
    public string QuestionText { get; set; } = default!;

    public int QuizId { get; set; }
    public Quiz? Quiz { get; set; }

    public ICollection<Answer>? Answers { get; set; }
    
}