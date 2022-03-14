
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.DAL;
using WebApp.Domain;

namespace WebApp.Pages.QuizAnswers
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<QuizAnswer>? QuizAnswer { get;set; }

        public async Task OnGetAsync()
        {
            QuizAnswer = await _context.QuizAnswers
                .Include(q => q.Answer)
                .Include(q => q.AnsweredQuiz).ToListAsync();
        }
    }
}
