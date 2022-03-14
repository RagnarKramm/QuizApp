
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
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int id)
        {
            QuizId = id;
            return Page();
        }

        [BindProperty]
        public AnsweredQuiz? AnsweredQuiz { get; set; }

        public int QuizId { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.AnsweredQuizzes.Add(AnsweredQuiz!);
            var quizId = AnsweredQuiz!.QuizId;
            await _context.SaveChangesAsync();

            var quiz = await _context.Quizzes.Include(i => i.Questions).FirstOrDefaultAsync(i => i.Id == quizId);
            if (quiz == null)
            {
                return NotFound();
            }
            return RedirectToPage("/Questions/QuestionForm", new {id = quiz.Questions!.First().Id, answeredQuizId = AnsweredQuiz.Id});
        }
    }
}
