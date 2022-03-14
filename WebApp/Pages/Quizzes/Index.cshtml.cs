
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
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Quiz>? Quiz { get;set; }

        public string? HeadlineSearch { get; set; }
        public string? AuthorSearch { get; set; }

        public async Task OnGetAsync(string? headlineSearch,string? authorSearch, string action)
        {
            if (action == "Clear")
            {
                headlineSearch = null;
                authorSearch = null;
            }
            
            HeadlineSearch = headlineSearch;
            AuthorSearch = authorSearch;
            var quizQuery = _context.Quizzes.AsQueryable();
        
            if (!string.IsNullOrWhiteSpace(authorSearch))
            {
                authorSearch = authorSearch.Trim();
                quizQuery = quizQuery.Where(i => i.Author.ToUpper().Contains(authorSearch.ToUpper()));
            }
            
            if (!string.IsNullOrWhiteSpace(headlineSearch))
            {
                headlineSearch = headlineSearch.Trim();
                quizQuery = quizQuery.Where(i => i.Headline.ToUpper().Contains(headlineSearch.ToUpper()));
            }
            
            Quiz = await quizQuery.ToListAsync();
        }
    }
}
