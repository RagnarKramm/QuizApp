using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.DAL;
using WebApp.Domain;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly AppDbContext _context;


    public IndexModel(ILogger<IndexModel> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;

    }

    public IList<Quiz>? Quiz { get;set; }
    public IList<AnsweredQuiz>? AnsweredQuiz { get;set; }


    public string? HeadlineSearch { get; set; }
    public string? AuthorSearch { get; set; }

    public async Task OnGetAsync(string? headlineSearch,string? authorSearch, string action)
    {
        AnsweredQuiz = await _context.AnsweredQuizzes
            .Include(a => a.Quiz).ToListAsync();
        
        
        if (action == "Clear")
        {
            headlineSearch = null;
            authorSearch = null;
        }
            
        HeadlineSearch = headlineSearch;
        AuthorSearch = authorSearch;
        var quizQuery = _context.Quizzes
            .Include(q => q.Questions)
            .Include(q => q.AnsweredQuizzes).AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(authorSearch))
        {
            authorSearch = authorSearch.Trim();
            quizQuery = quizQuery.Where(i => i.Author.ToUpper().Contains(authorSearch.ToUpper()));
        }
            
        if (!string.IsNullOrWhiteSpace(headlineSearch))
        {
            headlineSearch = headlineSearch.Trim();
            quizQuery = quizQuery.Where(i => i.Headline.ToUpper().Contains(headlineSearch.ToUpper()));
        }
            
        Quiz = await quizQuery.ToListAsync();
    }
}
