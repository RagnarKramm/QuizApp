using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.DAL;
using WebApp.Domain;

namespace WebApp.Pages.AnsweredQuizzes;

public class QuizStatistics : PageModel
{
    private readonly AppDbContext _context;

    public QuizStatistics(AppDbContext context)
    {
        _context = context;
    }

    
    public IList<AnsweredQuiz>? AnsweredQuizzes { get;set; }
    public Quiz? Quiz { get; set; }
    public bool HasQuestions { get; set; }

    public async Task OnGetAsync(int id)
    {
        Quiz = await _context.Quizzes
            .Include(i => i.Questions!)
            .ThenInclude(i => i.Answers)
            .FirstOrDefaultAsync(i => i.Id == id);

        AnsweredQuizzes = await _context.AnsweredQuizzes
            .Include(a => a.Quiz).Where(i => i.QuizId == id).ToListAsync();

        HasQuestions = false;
        foreach (var answered in AnsweredQuizzes)
        {
            if (answered.QuizQuestionsTotal > 0)
            {
                HasQuestions = true;
            }
        }
        
    }
}