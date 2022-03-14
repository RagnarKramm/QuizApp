
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.DAL;
using WebApp.Domain;

namespace WebApp.Pages.QuizAnswers
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["AnswerId"] = new SelectList(_context.Answers, "Id", "Id");
        ViewData["AnsweredQuizId"] = new SelectList(_context.AnsweredQuizzes, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public QuizAnswer? QuizAnswer { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.QuizAnswers.Add(QuizAnswer!);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
