using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MainlineUK.Data;
using MainlineUK.Models;

namespace MainlineUK.Pages.Stock
{
    public class EditModel : PageModel
    {
        private readonly MainlineUK.Data.ApplicationDbContext _context;

        public EditModel(MainlineUK.Data.ApplicationDbContext context)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(StocklistImport).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StocklistImportExists(StocklistImport.StocklistImportID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool StocklistImportExists(int id)
        {
            return _context.StocklistImport.Any(e => e.StocklistImportID == id);
        }
    }
}
