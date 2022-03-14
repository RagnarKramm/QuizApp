
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
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        public QuizAnswer? QuizAnswer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            QuizAnswer = await _context.QuizAnswers
                .Include(q => q.Answer)
                .Include(q => q.AnsweredQuiz).FirstOrDefaultAsync(m => m.Id == id);

            if (QuizAnswer == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
