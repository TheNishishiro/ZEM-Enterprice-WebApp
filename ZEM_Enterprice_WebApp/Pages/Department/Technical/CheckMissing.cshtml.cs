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

namespace ZEM_Enterprice_WebApp.Pages.Department.Technical
{
    [Authorize(Policy = "AdminOrTech")]
    public class CheckMissingModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public CheckMissingModel(ApplicationDbContext db)
        {
            _db = db;
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
        [Display(Name = "Data dodania")]
        [DataType(DataType.Date)]
        public DateTime Filter_Date { get; set; }

        public PaginatedList<Data.Tables.MissingFromTech> Data { get; set; }

        public async Task OnGetAsync()
        {
            var query = _db.MissingFromTech.AsNoTracking().AsQueryable();
            if(Filter_Date != null && Filter_Date != DateTime.MinValue)
            {
                query = query.Where(c => c.DataDodania.Date == Filter_Date.Date);
            }

            string[] sortings = {
                "Data dodania"
            };
            availableSortings = new SelectList(sortings);
            if (SortColumn != null)
            {
                switch (SortColumn)
                {
                    case "Data dodania":
                        if (OrderDescent)
                            query = query.OrderByDescending(c => c.DataDodania);
                        else
                            query = query.OrderBy(c => c.DataDodania);
                        break;
                }
            }

            Data = await PaginatedList<Data.Tables.MissingFromTech>.CreateAsync(query, CurrentPage, PageSize);
            return;
        }

        public async Task<IActionResult> OnPostDeleteRecordAsync(string id)
        {
            var rec = await _db.MissingFromTech.FindAsync(id);
            if (rec != null)
            {
                _db.MissingFromTech.Remove(rec);
                await _db.SaveChangesAsync();
            }

            await OnGetAsync();

            return Page();
        }
    }
}