
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

namespace WebApp.Pages.Answers
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            QuestionId = id;
            CanBeCorrect = true;

            var answers = await _context.Answers.Where(i => i.QuestionId == id).ToListAsync();

            foreach (var answer in answers)
            {
                if (answer.IsCorrect)
                {
                    CanBeCorrect = false;
                }
            }
            return Page();
        }

        [BindProperty]
        public Answer? Answer { get; set; }

        public bool CanBeCorrect { get; set; }
        public int QuestionId { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Answers.Add(Answer!);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Questions/Edit", new {id = Answer!.QuestionId});
        }
    }
}
