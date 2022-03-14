using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.DAL;
using WebApp.Domain;

namespace WebApp.Pages.Questions;

public class QuestionForm : PageModel
{
    private readonly AppDbContext _context;

    public QuestionForm(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Question? Question { get; set; }
    [BindProperty]
    public int AnsweredQuizId { get; set; }

    [BindProperty]
    public bool IsPoll { get; set; }
    
    //[BindProperty] public List<int> Answers { get; set; }
    
    
    public async Task<IActionResult> OnGetAsync(int? id, int? answeredQuizId)
    {
        if (id == null || answeredQuizId == null)
        {
            return NotFound();
        }

        Question = await _context.Questions
            .Include(q => q.Quiz)
            .Include(q => q.Answers).FirstOrDefaultAsync(m => m.Id == id);

        AnsweredQuizId = answeredQuizId ?? 0;
        if (Question == null)
        {
            return NotFound();
        }
            
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(List<int>? answerId)
    {
        if (answerId == null)
        {
            return RedirectToPage("/Questions/QuestionForm", new {id = Question!.Id});
        }
        Question = await _context.Questions
            .Include(q => q.Quiz)
            .Include(q => q.Answers).FirstOrDefaultAsync(m => m.Id == Question!.Id);
        if (IsPoll)
        {
            foreach (var id in answerId)
            {
                var answer = await _context.Answers.FirstAsync(i => i.Id == id);
                var answerQuiz = await _context.AnsweredQuizzes.FirstAsync(i => i.Id == AnsweredQuizId);
                var quizAnswer = new QuizAnswer
                {
                    AnsweredQuizId = AnsweredQuizId,
                    AnsweredQuiz = answerQuiz,
                    Answer = answer,
                    AnswerId = id
                };
                _context.QuizAnswers.Add(quizAnswer);
            }
            
        }
        else
        {
            var answer = await _context.Answers.FirstAsync(i => i.Id == answerId[0]);
            var answerQuiz = await _context.AnsweredQuizzes.FirstAsync(i => i.Id == AnsweredQuizId);
            var quizAnswer = new QuizAnswer
            {
                AnsweredQuizId = AnsweredQuizId,
                AnsweredQuiz = answerQuiz,
                Answer = answer,
                AnswerId = answerId[0]
            };
            _context.QuizAnswers.Add(quizAnswer);
        }
        await _context.SaveChangesAsync();


        var quizAnswers = await _context.QuizAnswers
            .Include(i => i.AnsweredQuiz)
                .ThenInclude(i => i!.Quiz)
            .Include(i=> i.Answer)
            .Where(a => a.AnsweredQuiz!.Id == AnsweredQuizId).ToListAsync();

        var questions = await _context.Questions.ToListAsync();
        var unansweredQuestions = new List<Question>();
        foreach (var question in questions)
        {
            if (question.QuizId != Question!.QuizId)
            {
                continue;
            }
            var isInList = false;
            foreach (var quizAnswer in quizAnswers)
            {
                if (question.Id == quizAnswer.Answer!.QuestionId)
                {
                    isInList = true;
                }
            }

            if (!isInList)
            {
                unansweredQuestions.Add(question);
            }
        }
        if (unansweredQuestions.Count < 1)
        {
            return RedirectToPage("/AnsweredQuizzes/QuizResult", new {answeredQuizId = AnsweredQuizId});
        }
        return RedirectToPage("/Questions/QuestionForm", new {id = unansweredQuestions.First().Id, answeredQuizId = AnsweredQuizId});
        
    }
}