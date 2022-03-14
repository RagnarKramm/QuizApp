
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
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Answer? Answer { get; set; }
        public bool CanBeCorrect { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Answer = await _context.Answers
                .Include(a => a.Question).FirstOrDefaultAsync(m => m.Id == id);

            if (Answer == null)
            {
                return NotFound();
            }
            
            CanBeCorrect = true;

            var answers = await _context.Answers.Where(i => i.QuestionId == Answer.QuestionId).ToListAsync();

            foreach (var answer in answers)
            {
                if (answer.IsCorrect)
                {
                    CanBeCorrect = false;
                }
            }
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

            _context.Attach(Answer!).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(Answer!.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Questions/Edit", new {id = Answer!.QuestionId});
        }

        private bool AnswerExists(int id)
        {
            return _context.Answers.Any(e => e.Id == id);
        }
    }
}
