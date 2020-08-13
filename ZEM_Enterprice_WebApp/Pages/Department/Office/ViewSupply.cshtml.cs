using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZEM_Enterprice_WebApp.Data;
using ZEM_Enterprice_WebApp.Utilities;

namespace ZEM_Enterprice_WebApp.Pages.Department.Office
{
    [Authorize(Policy = "CanViewSupply")]
    public class ViewSupplyModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public ViewSupplyModel(ApplicationDbContext db)
        {
            _db = db;

            var x = _db.Technical.GroupBy(c => c.PrzewodCiety).Select(c => new { Value = c.Key, Count = c.Count() }).OrderByDescending(c => c.Count);

        }

        public SelectList availableSortings { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 100;

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Sortowanie po kolumnie")]
        public string SortColumn { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Sortuj malejąco")]
        public bool OrderDescent { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Kod")]
        public string Filter_Kod { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Wiązka")]
        public string Filter_Wiazka { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Ilość")]
        public int? Filter_Ilosc { get; set; } 
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Data od")]
        [DataType(DataType.Date)]
        public DateTime Filter_DataFrom { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Data do")]
        [DataType(DataType.Date)]
        public DateTime Filter_DataTo { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Data utworzenia")]
        [DataType(DataType.Date)]
        public DateTime Filter_DateCreated { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Uwagi")]
        public string Filter_Uwagi { get; set; }

        public PaginatedList<Data.Tables.Dostawa> Data { get; set; }

        public async Task OnGetAsync()
        {
            if (Request.Cookies["pagesize"] != null)
                PageSize = int.Parse(Request.Cookies["pagesize"]);

            var query = _db.Dostawa.Include(c => c.Technical).AsNoTracking().AsQueryable();
            char separator = ',';

            if (Filter_Kod != null)
            {
                var options = Filter_Kod.Split(separator).Select(c => c.Replace("PLC", "").Trim());
                query = query.Where(c => options.Contains(c.Kod.Replace("PLC", "")));
            }
            if (Filter_Wiazka != null)
            {
                var options = Filter_Wiazka.Split(separator).Select(c => c.Trim());
                query = query.Where(c => options.Contains(c.Technical.Wiazka));
            }
            if (Filter_Ilosc != null)
                query = query.Where(c => c.Ilosc == Filter_Ilosc);
            if (Filter_DataFrom != DateTime.MinValue && Filter_DataTo != DateTime.MinValue)
                query = query.Where(c => c.Data.Date >= Filter_DataFrom.Date && c.Data.Date <= Filter_DataTo.Date);
            else if(Filter_DataFrom != DateTime.MinValue)
                query = query.Where(c => c.Data.Date == Filter_DataFrom.Date);
            if (Filter_DateCreated != DateTime.MinValue)
                query = query.Where(c => c.DataUtworzenia.Date == Filter_DateCreated.Date);
            if (Filter_Uwagi != null)
            {
                var options = Filter_Uwagi.Split(separator).Select(c => c.Trim());
                query = query.Where(c => options.Contains(c.Uwagi));
            }

            string[] sortings = { 
                "Kod", 
                "Ilość", 
                "Data", 
                "Data utworzenia"
            };
            availableSortings = new SelectList(sortings);

            if (SortColumn != null)
            {
                switch (SortColumn)
                {
                    case "Kod":
                        if(OrderDescent)
                            query = query.OrderByDescending(c => c.Kod);
                        else
                            query = query.OrderBy(c => c.Kod);
                        break;
                    case "Ilość":
                        if (OrderDescent)
                            query = query.OrderByDescending(c => c.Ilosc);
                        else
                            query = query.OrderBy(c => c.Ilosc);
                        break;
                    case "Data":
                        if (OrderDescent)
                            query = query.OrderByDescending(c => c.Data);
                        else
                            query = query.OrderBy(c => c.Data);
                        break;
                    case "Data utworzenia":
                        if (OrderDescent)
                            query = query.OrderByDescending(c => c.DataUtworzenia);
                        else
                            query = query.OrderBy(c => c.DataUtworzenia);
                        break;
                }
            }

            Data = await PaginatedList<Data.Tables.Dostawa>.CreateAsync(query, CurrentPage, PageSize);
            return;
        }

        public async Task<IActionResult> OnPostDeleteRecordAsync(string id)
        {
            var rec = await _db.Dostawa.FirstOrDefaultAsync(c => c.DostawaId == Guid.Parse(id));
            if (rec != null)
            {
                _db.VtToDostawa.RemoveRange(_db.VtToDostawa.Where(c => c.DostawaId == Guid.Parse(id)));
                _db.Dostawa.Remove(rec);
                
                await _db.SaveChangesAsync();
            }

            await OnGetAsync();

            return Page();
        }
    }
}