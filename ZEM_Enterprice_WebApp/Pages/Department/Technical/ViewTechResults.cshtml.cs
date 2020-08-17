using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZEM_Enterprice_WebApp.Data;
using ZEM_Enterprice_WebApp.Utilities;

namespace ZEM_Enterprice_WebApp.Pages.Department.Technical
{
    [Authorize(Policy = "CanViewVT")]
    public class ViewTechResultsModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public ViewTechResultsModel(ApplicationDbContext db)
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
        [Display(Name = "Rodzina")]
        public string Filter_Rodzina { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Litera rodziny")]
        public string Filter_LiteraRodziny { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Uwagi")]
        public string Filter_Uwagi { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Ilość przewodów")]
        public string Filter_IlePrzedowow { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Wiązka")]
        public string Filter_Wiazka { get; set; } 
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Kod wiązki")]
        public string Filter_KodWiazki { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Przewód Cięty")]
        public string Filter_PrzewodCiety { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "BIN")]
        public string Filter_BIN { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Pokaż usunięte")]
        public bool ShowDeleted { get; set; }

        public PaginatedList<Data.Tables.Technical> Data { get; set; }

        public async Task OnGetAsync()
        {
            CreatePage();
            return;
        }
        public async Task<IActionResult> OnPostDeleteRecordAsync(string id)
        {
            var rec = await _db.Technical.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.CietyWiazka == id);
            if (rec != null)
            {
                rec.Deleted = !rec.Deleted;
                if (rec.Deleted)
                    rec.DeleteDate = DateTime.Now.Date;
                else
                    rec.DeleteDate = null;
                _db.Update(rec);
                await _db.SaveChangesAsync();
            }

            await OnGetAsync();

            return Page();
        }
        public void CreatePage()
        {
            if (Request.Cookies["pagesize"] != null)
                PageSize = int.Parse(Request.Cookies["pagesize"]);

            IQueryable<Data.Tables.Technical> query;

            if(!ShowDeleted)
                query = _db.Technical.AsNoTracking().AsQueryable();
            else
                query = _db.Technical.AsNoTracking().IgnoreQueryFilters().AsQueryable();

            char separator = ',';

            if (Filter_Rodzina != null)
            {
                var options = Filter_Rodzina.Split(separator).Select(c => c.Trim());
                query = query.Where(c => options.Contains(c.Rodzina));
            }
            if (Filter_Wiazka != null)
            {
                var options = Filter_Wiazka.Split(separator).Select(c => c.Trim());
                query = query.Where(c => options.Contains(c.Wiazka));
            }
            if (Filter_KodWiazki != null)
            {
                var options = Filter_KodWiazki.Split(separator).Select(c => c.Trim());
                query = query.Where(c => options.Contains(c.KodWiazki));
            }
            if (Filter_PrzewodCiety != null)
            {
                var options = Filter_PrzewodCiety.Split(separator).Select(c => c.Replace("PLC", "").Trim());
                query = query.Where(c => options.Contains(c.PrzewodCiety));
            }
            if (Filter_BIN != null)
            {
                var options = Filter_BIN.Split(separator).Select(c => c.Trim());
                query = query.Where(c => options.Contains(c.BIN));
            }
            if (Filter_LiteraRodziny != null)
            {
                var options = Filter_LiteraRodziny.Split(separator).Select(c => c.Trim());
                query = query.Where(c => options.Contains(c.LiterRodziny));
            }
            if (Filter_Uwagi != null)
            {
                var options = Filter_Uwagi.Split(separator).Select(c => c.Trim());
                query = query.Where(c => options.Contains(c.Uwagi));
            }
            if (Filter_IlePrzedowow != null)
            {
                var options = Filter_IlePrzedowow.Split(separator).Select(c => c.Trim());
                query = query.Where(c => options.Contains(c.IlePrzewodow));
            }

            string[] sortings = {
                "Rodzina",
                "Wiązka",
                "Litera rodziny",
                "Kod wiązki",
                "Przew. Cięty",
                "BIN"
            };
            availableSortings = new SelectList(sortings);

            if (SortColumn != null)
            {
                switch (SortColumn)
                {
                    case "Rodzina":
                        if (OrderDescent)
                            query = query.OrderByDescending(c => c.Rodzina);
                        else
                            query = query.OrderBy(c => c.Rodzina);
                        break;
                    case "Wiązka":
                        if (OrderDescent)
                            query = query.OrderByDescending(c => c.Wiazka);
                        else
                            query = query.OrderBy(c => c.Wiazka);
                        break;
                    case "Litera rodziny":
                        if (OrderDescent)
                            query = query.OrderByDescending(c => c.LiterRodziny);
                        else
                            query = query.OrderBy(c => c.LiterRodziny);
                        break;
                    case "Kod wiązki":
                        if (OrderDescent)
                            query = query.OrderByDescending(c => c.KodWiazki);
                        else
                            query = query.OrderBy(c => c.KodWiazki);
                        break;
                    case "Przew. Cięty":
                        if (OrderDescent)
                            query = query.OrderByDescending(c => c.PrzewodCiety);
                        else
                            query = query.OrderBy(c => c.PrzewodCiety);
                        break;
                    case "BIN":
                        if (OrderDescent)
                            query = query.OrderByDescending(c => c.BIN);
                        else
                            query = query.OrderBy(c => c.BIN);
                        break;
                }
            }

            Data = PaginatedList<Data.Tables.Technical>.CreateAsync(query, CurrentPage, PageSize).Result;
        }

        public async Task<IActionResult> OnPostPageDownloadCsvAsync()
        {
            CreatePage();
            MemoryStream ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.AutoFlush = true;
            var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            await csv.WriteRecordsAsync(Data);

            await ms.FlushAsync();
            await csv.FlushAsync();
            ms.Position = 0;
            return File(ms, "text/csv", $"baza-techniczna-strona-{Data.PageIndex}.csv");
        }

        public async Task<IActionResult> OnPostDownloadCsvAsync()
        {
            MemoryStream ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.AutoFlush = true;
            var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(await _db.Technical.ToListAsync());
            await ms.FlushAsync();
            await csv.FlushAsync();
            ms.Position = 0;

            return File(ms, "text/csv", "baza-techniczna.csv");
        }
    }
}