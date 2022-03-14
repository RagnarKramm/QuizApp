
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

namespace WebApp.Pages.QuizAnswers
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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
           ViewData["AnswerId"] = new SelectList(_context.Answers, "Id", "Id");
           ViewData["AnsweredQuizId"] = new SelectList(_context.AnsweredQuizzes, "Id", "Id");
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

            _context.Attach(QuizAnswer!).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizAnswerExists(QuizAnswer!.Id))
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

        private bool QuizAnswerExists(int id)
        {
            return _context.QuizAnswers.Any(e => e.Id == id);
        }
    }
}
