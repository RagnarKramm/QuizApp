
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.DAL;
using WebApp.Domain;

namespace WebApp.Pages.AnsweredQuizzes
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
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

            AnsweredQuiz = await _context.AnsweredQuizzes
                .Include(a => a.Quiz).FirstOrDefaultAsync(m => m.Id == id);

            if (AnsweredQuiz == null)
            {
                return NotFound();
            }
           ViewData["QuizId"] = new SelectList(_context.Quizzes, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(AnsweredQuiz!).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnsweredQuizExists(AnsweredQuiz!.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AnsweredQuizExists(int id)
        {
            return _context.AnsweredQuizzes.Any(e => e.Id == id);
        }
    }
}
