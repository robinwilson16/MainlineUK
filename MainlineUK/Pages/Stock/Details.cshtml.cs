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
    public class DetailsModel : PageModel
    {
        private readonly MainlineUK.Data.ApplicationDbContext _context;

        public DetailsModel(MainlineUK.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
