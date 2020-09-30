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
    public class CheckKanbansModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public CheckKanbansModel(ApplicationDbContext db)
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

        public PaginatedList<Data.Tables.ScannedKanban> Data { get; set; }

        public async Task OnGetAsync()
        {
            if (Request.Cookies["pagesize"] != null)
                PageSize = int.Parse(Request.Cookies["pagesize"]);

            var query = _db.ScannedKanbans.AsNoTracking().AsQueryable();
            if (Filter_Date != null && Filter_Date != DateTime.MinValue)
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

            Data = await PaginatedList<Data.Tables.ScannedKanban>.CreateAsync(query, CurrentPage, PageSize);
            return;
        }

        public async Task<IActionResult> OnPostDownloadCsvAsync()
        {
            return File(await CSVDownloader.OnPostDownloadCsvAsync(_db, _db.ScannedKanbans.AsQueryable()), "text/csv", $"kanbany-{DateTime.Now.Date}-{DateTime.Now.Month}-{DateTime.Now.Year}.csv");
        }

        public async Task<IActionResult> OnPostDeleteRecordAsync(long id)
        {
            var rec = await _db.ScannedKanbans.FindAsync(id);
            if (rec != null)
            {
                _db.ScannedKanbans.Remove(rec);
                await _db.SaveChangesAsync();
            }

            await OnGetAsync();

            return Page();
        }
    }
}