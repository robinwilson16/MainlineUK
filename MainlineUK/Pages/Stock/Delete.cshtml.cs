using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MainlineUK.Data;
using MainlineUK.Models;

namespace MainlineUK.Pages.Stock
{
    public class DeleteModel : PageModel
    {
        private readonly MainlineUK.Data.ApplicationDbContext _context;

        public DeleteModel(MainlineUK.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public StocklistImport StocklistImport { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StocklistImport = await _context.StocklistImport.FirstOrDefaultAsync(m => m.StocklistImportID == id);

            if (StocklistImport == null)
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

            StocklistImport = await _context.StocklistImport.FindAsync(id);

            if (StocklistImport != null)
            {
                _context.StocklistImport.Remove(StocklistImport);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
