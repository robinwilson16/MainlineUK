using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MainlineUK.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MainlineUK.Data.ApplicationDbContext _context;

        public IndexModel(MainlineUK.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<SelectListItem> BodyTypes { get; set; }
        public IList<SelectListItem> Makes { get; set; }
        public IList<SelectListGroup> ModelMakes { get; set; }
        public IList<SelectListItem> Models { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public int? MinBudget { get; set; }
        public int? MaxBudget { get; set; }

        public async Task OnGetAsync()
        {
            //Select Lists
            BodyTypes = await _context.StocklistImport
                .Select(
                    s => new SelectListItem
                    {
                        Value = s.BodyType,
                        Text = s.BodyType
                    }
                )
                .Distinct()
                .ToListAsync();

            Makes = await _context.StocklistImport
                .Select(
                    s => new SelectListItem
                    {
                        Value = s.Make,
                        Text = s.Make
                    }
                )
                .Distinct()
                .ToListAsync();

            ModelMakes = await _context.StocklistImport
                .Select(
                    s => new SelectListGroup
                    {
                        Name = s.Make
                    }
                )
                .Distinct()
                .ToListAsync();

            Models = (await _context.StocklistImport
                .ToListAsync())
                .GroupBy(grp => new { grp.Make, grp.Model, grp.BodyType })
                .Select(
                    s => new SelectListItem
                    {
                        Value = s.Key.Model,
                        Text = s.Key.Model + " (" + s.Key.BodyType + ")",
                        Group = ModelMakes.SingleOrDefault(m => m.Name == s.Key.Make)
                    }
                )
                .OrderBy(s => s.Group.Name)
                    .ThenBy(s => s.Text)
                .ToList();

            MinPrice = null;
            MaxPrice = null;
            MinBudget = null;
            MaxBudget = null;
        }
    }
}