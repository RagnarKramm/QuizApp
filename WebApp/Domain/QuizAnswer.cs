namespace WebApp.Domain;

public class QuizAnswer : BaseEntity
{
    public int AnsweredQuizId { get; set; }
    public AnsweredQuiz? AnsweredQuiz { get; set; }

    public int AnswerId { get; set; }
    public Answer? Answer { get; set; }
}