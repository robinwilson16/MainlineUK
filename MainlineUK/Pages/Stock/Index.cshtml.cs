using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MainlineUK.Data;
using MainlineUK.Models;
using MainlineUK.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MainlineUK.Pages.Stock
{
    public class IndexModel : PageModel
    {
        private readonly MainlineUK.Data.ApplicationDbContext _context;

        public IndexModel(MainlineUK.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public string CurrentFilterID { get; set; }
        public string CurrentSortID { get; set; }
        public string SortDateAdded { get; set; }
        public string SortMileage { get; set; }
        public string SortRegYear { get; set; }
        public string SortPrice { get; set; }

        public PaginatedList<StocklistImport> StocklistImport { get;set; }
        public IList<StocklistImport> StocklistImport2 { get; set; }
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
            string currentFilter, 
            string searchString,
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
            //Set filter
            Filters = new List<Filter>();
            if (!String.IsNullOrEmpty(make))
            {
                Filters.Add(new Filter
                {
                    FilterExp = "make=" + make
                });
            }

            if (!String.IsNullOrEmpty(model))
            {
                //Replace previously replaced spaces
                model = model.Replace("_", " ");

                Filters.Add(new Filter
                {
                    FilterExp = "model=" + model
                });
            }

            if (min_price >= 0)
            {
                Filters.Add(new Filter
                {
                    FilterExp = "min_price=" + min_price
                });
            }

            if (max_price >= 0)
            {
                Filters.Add(new Filter
                {
                    FilterExp = "max_price=" + max_price
                });
            }

            if (mileage >= 0)
            {
                Filters.Add(new Filter
                {
                    FilterExp = "mileage=" + mileage
                });
            }

            if (!String.IsNullOrEmpty(transmission))
            {
                Filters.Add(new Filter
                {
                    FilterExp = "transmission=" + transmission
                });
            }

            if (!String.IsNullOrEmpty(fuel_type))
            {
                Filters.Add(new Filter
                {
                    FilterExp = "fuel_type=" + fuel_type
                });
            }

            if (!String.IsNullOrEmpty(body_type))
            {
                //Replace previously replaced spaces
                body_type = body_type.Replace("_", " ");

                Filters.Add(new Filter
                {
                    FilterExp = "body_type=" + body_type
                });
            }

            //Select Lists
            Makes = await _context.StocklistImport
                .Select(
                    s => new SelectListItem {
                        Value = s.Make,
                        Text = s.Make,
                        Selected = s.Make == make
                    }
                )
                .Distinct()
                .ToListAsync();

            ModelMakes = await _context.StocklistImport
                .Select(
                    s => new SelectListGroup {
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

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            //Add a default sort order
            if (sortOrder == null)
            {
                sortOrder = "Recent";
            }

            CurrentFilterID = searchString;
            CurrentSortID = sortOrder;

            IQueryable<StocklistImport> stocklistImportIQ = from s in _context.StocklistImport
                                                            select s;

            //Process searches
            if (!String.IsNullOrEmpty(make))
            {
                stocklistImportIQ = stocklistImportIQ.Where(s => s.Make.Equals(make));
            }

            if (!String.IsNullOrEmpty(model))
            {
                stocklistImportIQ = stocklistImportIQ.Where(s => s.Model.Equals(model));
            }

            if (min_price >= 0)
            {
                stocklistImportIQ = stocklistImportIQ.Where(s => s.Price >= min_price);
            }

            if (max_price >= 0)
            {
                stocklistImportIQ = stocklistImportIQ.Where(s => s.Price <= max_price);
            }

            if (mileage >= 0)
            {
                stocklistImportIQ = stocklistImportIQ.Where(s => s.Mileage <= mileage);
            }

            if (!String.IsNullOrEmpty(transmission))
            {
                stocklistImportIQ = stocklistImportIQ.Where(s => s.Transmission.Equals(transmission));
            }

            if (!String.IsNullOrEmpty(fuel_type))
            {
                stocklistImportIQ = stocklistImportIQ.Where(s => s.FuelType.Equals(fuel_type));
            }

            if (!String.IsNullOrEmpty(body_type))
            {
                stocklistImportIQ = stocklistImportIQ.Where(s => s.BodyType.Equals(body_type));
            }

            switch (sortOrder)
            {
                case "Latest":
                    stocklistImportIQ = stocklistImportIQ.OrderByDescending(s => s.CreatedDate).Take(20);
                    break;
                case "Recent":
                    stocklistImportIQ = stocklistImportIQ.OrderByDescending(s => s.CreatedDate);
                    break;
                case "LowMileage":
                    stocklistImportIQ = stocklistImportIQ.OrderBy(s => s.Mileage);
                    break;
                case "RecentPlates":
                    stocklistImportIQ = stocklistImportIQ.OrderByDescending(s => s.RegCode);
                    break;
                case "LowPrice":
                    stocklistImportIQ = stocklistImportIQ.OrderBy(s => s.Price);
                    break;
                default:
                    stocklistImportIQ = stocklistImportIQ.OrderByDescending(s => s.CreatedDate);
                    break;
            }

            //StocklistImport = await _context.StocklistImport
            //    .Include(s => s.Photo)
            //    .ToListAsync();

            //StocklistImport = await stocklistImportIQ.AsNoTracking()
            //    .Include(s => s.Photo)
            //    .ToListAsync();

            int pageSize = 16;
            StocklistImport = await PaginatedList<StocklistImport>.CreateAsync(
                stocklistImportIQ.AsNoTracking()
                    .Include(s => s.Photo), 
                pageIndex ?? 1, 
                pageSize
                );
        }

        public async Task<IActionResult> OnGetJsonAsync(
            string sortOrder,
            string currentFilter,
            string searchString,
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
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            //Add a default sort order
            if (sortOrder == null)
            {
                sortOrder = "Recent";
            }

            CurrentFilterID = searchString;
            CurrentSortID = sortOrder;

            IQueryable<StocklistImport> stocklistImportIQ = from s in _context.StocklistImport
                                                            select s;

            //Process searches
            if (!String.IsNullOrEmpty(make))
            {
                stocklistImportIQ = stocklistImportIQ.Where(s => s.Make.Equals(make));
            }

            if (!String.IsNullOrEmpty(model))
            {
                stocklistImportIQ = stocklistImportIQ.Where(s => s.Model.Equals(model));
            }

            if (min_price >= 0)
            {
                stocklistImportIQ = stocklistImportIQ.Where(s => s.Price >= min_price);
            }

            if (max_price >= 0)
            {
                stocklistImportIQ = stocklistImportIQ.Where(s => s.Price <= max_price);
            }

            if (mileage >= 0)
            {
                stocklistImportIQ = stocklistImportIQ.Where(s => s.Mileage <= mileage);
            }

            if (!String.IsNullOrEmpty(transmission))
            {
                stocklistImportIQ = stocklistImportIQ.Where(s => s.Transmission.Equals(transmission));
            }

            if (!String.IsNullOrEmpty(fuel_type))
            {
                stocklistImportIQ = stocklistImportIQ.Where(s => s.FuelType.Equals(fuel_type));
            }

            if (!String.IsNullOrEmpty(body_type))
            {
                stocklistImportIQ = stocklistImportIQ.Where(s => s.BodyType.Equals(body_type));
            }

            switch (sortOrder)
            {
                case "Latest":
                    stocklistImportIQ = stocklistImportIQ.OrderByDescending(s => s.CreatedDate).Take(20);
                    break;
                case "Recent":
                    stocklistImportIQ = stocklistImportIQ.OrderByDescending(s => s.CreatedDate);
                    break;
                case "LowMileage":
                    stocklistImportIQ = stocklistImportIQ.OrderBy(s => s.Mileage);
                    break;
                case "RecentPlates":
                    stocklistImportIQ = stocklistImportIQ.OrderByDescending(s => s.RegCode);
                    break;
                case "LowPrice":
                    stocklistImportIQ = stocklistImportIQ.OrderBy(s => s.Price);
                    break;
                default:
                    stocklistImportIQ = stocklistImportIQ.OrderByDescending(s => s.CreatedDate);
                    break;
            }

            StocklistImport2 = await stocklistImportIQ.AsNoTracking()
                .Include(s => s.Photo)
                .ToListAsync();

            //int pageSize = 16;
            //StocklistImport = await PaginatedList<StocklistImport>.CreateAsync(
            //    stocklistImportIQ.AsNoTracking()
            //        .Include(s => s.Photo),
            //    pageIndex ?? 1,
            //    pageSize
            //    );


            var collectionWrapper = new
            {
                Stock = StocklistImport2
            };

            return new JsonResult(collectionWrapper);
        }

        public static string FormatNumberPlate(string numberPlate)
        {
            if(numberPlate.Length == 7)
            {
                numberPlate = numberPlate.Substring(0, 4) + " " + numberPlate.Substring(4, 3);
            }

            return numberPlate;
        }
    }
}
