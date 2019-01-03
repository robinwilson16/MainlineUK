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
using Microsoft.AspNetCore.Http;

namespace MainlineUK.Pages.Stock
{
    public class DetailsModel : PageModel
    {
        private readonly MainlineUK.Data.ApplicationDbContext _context;
        public string IVendiUsername { get; set; }
        public string IVendiQuoteeUID { get; set; }
        public string DomainName { get; set; }

        public DetailsModel(MainlineUK.Data.ApplicationDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            IVendiUsername = configuration["iVendi:Username"];
            IVendiQuoteeUID = configuration["iVendi:QuoteeUID"];
            DomainName = httpContextAccessor.HttpContext.Request.Scheme + "://" + httpContextAccessor.HttpContext.Request.Host.Value;
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
