using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MainlineUK.Data;
using MainlineUK.Models;
using Humanizer;
using Microsoft.Extensions.Configuration;

namespace MainlineUK.Pages.Stock
{
    public class DetailsModel : PageModel
    {
        private readonly MainlineUK.Data.ApplicationDbContext _context;
        public string ivendiUsername { get; set; }
        public string ivendiQuoteeUID { get; set; }

        public DetailsModel(MainlineUK.Data.ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            ivendiUsername = configuration["iVendi:Username"];
            ivendiQuoteeUID = configuration["iVendi:QuoteeUID"];
        }

        public StocklistImport StocklistImport { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StocklistImport = await _context.StocklistImport
                .Include(m => m.Photo)
                .FirstOrDefaultAsync(m => m.StocklistImportID == id);

            if (StocklistImport == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
