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
    public class IndexModel : PageModel
    {
        private readonly MainlineUK.Data.ApplicationDbContext _context;

        public IndexModel(MainlineUK.Data.ApplicationDbContext context)
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
        public IList<SelectListItem> PricesFrom { get; set; }
        public IList<SelectListItem> PricesTo { get; set; }
        public IList<SelectListItem> Mileage { get; set; }
        public IList<SelectListItem> Transmission { get; set; }
        public IList<SelectListItem> FuelType { get; set; }
        public IList<SelectListItem> BodyType { get; set; }
        public IList<Filter> Filters { get; set; }

        public async Task OnGetAsync(
            string sortOrder,
            string make,
            string model,
            int? min_price,
            int? max_price,
            int? mileage,
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

            PricesFrom = Enumerable.Range(0, 26)
                .Select(
                    s => new SelectListItem
                    {
                        Value = (s * 1000).ToString(),
                        Text = (s * 1000).ToString("C0"),
                        Selected = (s * 1000) == min_price
                    }
                )
                .Distinct()
                .ToList();

            PricesTo = Enumerable.Range(0, 26)
                .Select(
                    s => new SelectListItem
                    {
                        Value = (s * 1000).ToString(),
                        Text = (s * 1000).ToString("C0"),
                        Selected = (s * 1000) == max_price
                    }
                )
                .Distinct()
                .ToList();

            Mileage = Enumerable.Range(2, 9)
                .Select(
                    s => new SelectListItem
                    {
                        Value = (s * 10000).ToString(),
                        Text = "Up to " + (s * 10000).ToString("N0") + " miles",
                        Selected = (s * 10000) == mileage
                    }
                )
                .Distinct()
                .ToList();

            Transmission = await _context.StocklistImport
                .Select(
                    s => new SelectListItem
                    {
                        Value = s.Transmission,
                        Text = s.Transmission,
                        Selected = s.Transmission == transmission
                    }
                )
                .Distinct()
                .ToListAsync();

            FuelType = await _context.StocklistImport
                .Select(
                    s => new SelectListItem
                    {
                        Value = s.FuelType,
                        Text = s.FuelType,
                        Selected = s.FuelType == fuel_type
                    }
                )
                .Distinct()
                .ToListAsync();

            BodyType = await _context.StocklistImport
                .Select(
                    s => new SelectListItem
                    {
                        Value = s.BodyType,
                        Text = s.BodyType,
                        Selected = s.BodyType == body_type
                    }
                )
                .Distinct()
                .ToListAsync();

            //Add a default sort order
            if (sortOrder == null)
            {
                sortOrder = "Recent";
            }

            CurrentSortID = sortOrder;
        }
    }
}
