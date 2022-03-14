
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.DAL;
using WebApp.Domain;

namespace WebApp.Pages.Questions
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
        public Question? Question { get; set; }

        public int QuizId { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Questions.Add(Question!);
            await _context.SaveChangesAsync();
            
            var questionId = _context.Questions.Max(i => i.Id);

            return RedirectToPage("/Questions/Edit", new {id = questionId});

        }
    }
}
