using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain;

public class Quiz : BaseEntity
{
    [MaxLength(128)]
    [Display(Name = "Quiz name/headline")]
    public string Headline { get; set; } = default!;

    [MaxLength(64)]
    public string Author { get; set; } = default!;

    public ICollection<Question>? Questions { get; set; }
    
    public ICollection<AnsweredQuiz>? AnsweredQuizzes { get; set; }

}