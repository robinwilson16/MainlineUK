using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainlineUK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MainlineUK.Pages
{
    public class StocklistModel : PageModel
    {
        private readonly MainlineUK.Data.ApplicationDbContext _context;

        public StocklistModel(MainlineUK.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public string CurrentSortID { get; set; }
        public string SortDateAdded { get; set; }
        public string SortMileage { get; set; }
        public string SortRegYear { get; set; }
        public string SortPrice { get; set; }

        public IList<SelectListItem> Makes { get; set; }
        public IList<SelectListGroup> ModelMakes { get; set; }
        public IList<SelectListItem> Models { get; set; }
        public IList<Filter> Filters { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public int? MinBudget { get; set; }
        public int? MaxBudget { get; set; }

        public async Task OnGetAsync(
            string sortOrder,
            string make,
            string model,
            string min_price,
            string max_price,
            string min_budget,
            string max_budget,
            string mileage,
            string transmission,
            string fuel_type,
            string body_type,
            int? pageIndex
            )
        {
            //Select Lists
            Makes = await _context.StocklistImport
                .Select(
                    s => new SelectListItem
                    {
                        Value = s.Make,
                        Text = s.Make,
                        Selected = s.Make == make
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

            Models = _context.StocklistImport
                .GroupBy(grp => new { grp.Make, grp.Model })
                .ToList() /*Fix for Net Core 2.1 to avoid must be reducible node error*/
                .Select(
                    s => new SelectListItem
                    {
                        Value = s.Key.Model,
                        Text = s.Key.Model,
                        Selected = s.Key.Model == model,
                        Group = ModelMakes.SingleOrDefault(m => m.Name == s.Key.Make)
                    }
                )
                .Where(s => s.Group.Name.Equals(make) || make == null)
                .OrderBy(s => s.Group.Name)
                    .ThenBy(s => s.Text)
                .ToList();

            MinPrice = Shared.NumberFunctions.CurrencyToInt(min_price);
            MaxPrice = Shared.NumberFunctions.CurrencyToInt(max_price);
            MinBudget = Shared.NumberFunctions.CurrencyToInt(min_budget);
            MaxBudget = Shared.NumberFunctions.CurrencyToInt(max_budget);

            //Add a default sort order
            if (sortOrder == null)
            {
                sortOrder = "Recent";
            }

            CurrentSortID = sortOrder;
        }
    }
}
