
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.DAL;
using WebApp.Domain;

namespace WebApp.Pages.AnsweredQuizzes
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<AnsweredQuiz>? AnsweredQuiz { get;set; }

        public async Task OnGetAsync()
        {
            AnsweredQuiz = await _context.AnsweredQuizzes
                .Include(a => a.Quiz).ToListAsync();
        }
    }
}
