using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MainlineUK.Data;
using MainlineUK.Models;

namespace MainlineUK.Pages.Stock
{
    public class CreateModel : PageModel
    {
        private readonly MainlineUK.Data.ApplicationDbContext _context;

        public CreateModel(MainlineUK.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public StocklistImport StocklistImport { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.StocklistImport.Add(StocklistImport);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}