using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.DAL;
using WebApp.Domain;

namespace WebApp.Pages.QuizAnswers;

public class QuizResult : PageModel
{
    private readonly AppDbContext _context;

    public QuizResult(AppDbContext context)
    {
        _context = context;
    }

    public AnsweredQuiz? AnsweredQuiz { get; set; }
    public IList<Question>? QuizQuestions { get; set; }
    public IList<Answer>? UserAnswers { get; set; }

    public async Task<IActionResult> OnGetAsync(int answeredQuizId)
    {
        AnsweredQuiz = await _context.AnsweredQuizzes
            .Include(i => i.QuizAnswers)
            .Include(i => i.Quiz)
                .ThenInclude(i => i!.Questions)!
                    .ThenInclude(i => i.Answers)
            .FirstOrDefaultAsync(i => i.Id == answeredQuizId);

        QuizQuestions = await _context.Questions
            .Include(i => i.Quiz)
            .Where(q => q.Quiz!.Id == AnsweredQuiz!.QuizId).ToListAsync();

        var answers = await _context.Answers
            .Include(i => i.QuizAnswers)
            .ToListAsync();
        UserAnswers = new List<Answer>();
        
        foreach (var answer in answers)
        {
            foreach (var quizAnswer in answer.QuizAnswers!)
            {
                if (quizAnswer.AnsweredQuizId == answeredQuizId)
                {
                    UserAnswers.Add(answer);
                }
            }
        }
        
        var correctAnswers = 0;
        var quizQuestionAmount = 0;
        var pollQuestions = 0;
        
        foreach (var question in AnsweredQuiz!.Quiz!.Questions!)
        {
            var isPollQuestion = true;
            foreach (var answer in question.Answers!)
            {
                if (answer.IsCorrect)
                {
                    isPollQuestion = false;
                }
            }

            if (!isPollQuestion)
            {
                quizQuestionAmount++;
                
                if (UserAnswers!.First(i => i.QuestionId == question.Id).IsCorrect)
                {
                    correctAnswers++;
                }
            }
            else
            {
                pollQuestions++;
            }
            
        }

        AnsweredQuiz.PollQuestions = pollQuestions;
        AnsweredQuiz.QuizQuestionsCorrect = correctAnswers;
        AnsweredQuiz.QuizQuestionsTotal = quizQuestionAmount;
        await _context.SaveChangesAsync();
        
        return Page();
    }
}