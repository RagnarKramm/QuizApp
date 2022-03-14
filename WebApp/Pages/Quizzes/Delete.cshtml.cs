
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.DAL;
using WebApp.Domain;

namespace WebApp.Pages.Quizzes
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Quiz? Quiz { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Quiz = await _context.Quizzes.Include(q => q.Questions).FirstOrDefaultAsync(m => m.Id == id);

            if (Quiz == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Quiz = await _context.Quizzes
                .Include(q => q.Questions!)
                    .ThenInclude(q => q.Answers!)
                    .ThenInclude(a => a.QuizAnswers)
                .Include(i => i.AnsweredQuizzes!)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (Quiz != null)
            {
                if (Quiz.Questions != null)
                {
                    foreach (var question in Quiz.Questions)
                    {
                        if (question.Answers != null)
                        {
                            foreach (var answer in question.Answers)
                            {
                                if (answer.QuizAnswers != null)
                                {
                                    foreach (var quizAnswer in answer.QuizAnswers)
                                    {
                                        _context.QuizAnswers.Remove(quizAnswer);
                                    }
                                }
                                _context.Answers.Remove(answer);
                            }
                        }
                        _context.Questions.Remove(question);
                    }
                }

                if (Quiz.AnsweredQuizzes != null)
                {
                    foreach (var answered in Quiz.AnsweredQuizzes)
                    {
                        _context.AnsweredQuizzes.Remove(answered);
                    }
                }

                _context.Quizzes.Remove(Quiz);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Index");
        }
    }
}
