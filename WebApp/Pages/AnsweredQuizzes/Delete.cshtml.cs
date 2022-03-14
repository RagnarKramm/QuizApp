
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
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AnsweredQuiz? AnsweredQuiz { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AnsweredQuiz = await _context.AnsweredQuizzes.FindAsync(id);

            if (AnsweredQuiz != null)
            {
                _context.AnsweredQuizzes.Remove(AnsweredQuiz);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AnsweredQuiz = await _context.AnsweredQuizzes.FindAsync(id);

            if (AnsweredQuiz != null)
            {
                _context.AnsweredQuizzes.Remove(AnsweredQuiz);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
