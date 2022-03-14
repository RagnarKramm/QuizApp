
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
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        public AnsweredQuiz? AnsweredQuiz { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AnsweredQuiz = await _context.AnsweredQuizzes
                .Include(a => a.Quiz).FirstOrDefaultAsync(m => m.Id == id);

            if (AnsweredQuiz == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
