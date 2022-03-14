
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.DAL;
using WebApp.Domain;

namespace WebApp.Pages.Questions
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Question? Question { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Question = await _context.Questions
                .Include(q => q.Quiz).Include(q => q.Answers).FirstOrDefaultAsync(m => m.Id == id);

            if (Question == null)
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

            Question = await _context.Questions
                .Include(q => q.Answers!)
                .ThenInclude(a => a.QuizAnswers).FirstOrDefaultAsync(i => i.Id == id);

            if (Question != null)
            {
                if (Question.Answers != null)
                {
                    foreach (var answer in Question.Answers)
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
                _context.Questions.Remove(Question);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Quizzes/Edit", new {id = Question!.QuizId});
        }
    }
}
