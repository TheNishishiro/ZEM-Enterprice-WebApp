using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using ZEM_Enterprice_WebApp.Data;
using ZEM_Enterprice_WebApp.Data.Tables;
using ZEM_Enterprice_WebApp.Utilities;

namespace ZEM_Enterprice_WebApp.Pages.Department.Office
{
    [Authorize(Policy = "CanViewVTScans")]
    public class ViewVTMagazynModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public ViewVTMagazynModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public SelectList availableSortings { get; set; }
        public SelectList completeFilter { get; set; }
        public SelectList declaredFilter { get; set; }

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
        [Display(Name = "Numer kompletu")]
        public string Filter_NumerKompletu { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Sztuki zeskanowane")]
        public string Filter_SztukiZeskanowane { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Wiązka")]
        public string Filter_Wiazka { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Przewód Cięty")]
        public string Filter_KodCiety { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Pracownik")]
        public string Filter_Pracownik { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Dokument dostawy")]
        public string Filter_DokDostawy { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Dokument dostawy dopis")]
        public string Filter_DokDostawyDopis { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Litera rodziny")]
        public string Filter_LiteraRodziny { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Kod wiązki")]
        public string Filter_KodWiazki { get; set; }
        #region DATY
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Data utworzenia od")]
        [DataType(DataType.Date)]
        public DateTime Filter_DataUtworzeniaFrom { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Data dopisu od")]
        [DataType(DataType.Date)]
        public DateTime Filter_DataDopisuFrom { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Data dostawy od")]
        [DataType(DataType.Date)]
        public DateTime Filter_DataDostawyFrom { get; set; }


        [BindProperty(SupportsGet = true)]
        [Display(Name = "Data utworzenia do")]
        [DataType(DataType.Date)]
        public DateTime Filter_DataUtworzeniaTo { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Data dopisu do")]
        [DataType(DataType.Date)]
        public DateTime Filter_DataDopisuTo { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Data dostawy do")]
        [DataType(DataType.Date)]
        public DateTime Filter_DataDostawyTo { get; set; }
        #endregion

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Uwagi")]
        public string Filter_Uwagi { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Filtr kompletu")]
        public string Filter_Komplet { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Filtr deklaracji")]
        public string Filter_Deklaracja { get; set; }

        public PaginatedList<Data.Tables.VTMagazyn> Data { get; set; }

        public async Task OnGetAsync()
        {
            if (Request.Cookies["pagesize"] != null)
                PageSize = int.Parse(Request.Cookies["pagesize"]);

            string[] filterOptions =
            {
                "Prawda",
                "Fałsz",
            };
            completeFilter = new SelectList(filterOptions);
            declaredFilter = new SelectList(filterOptions);

            var query = _db.VTMagazyn.AsNoTracking().Include(c=>c.Technical).AsNoTracking().AsQueryable();
            char separator = ',';
            if (Filter_LiteraRodziny != null)
            {
                var options = Filter_LiteraRodziny.Split(separator).Select(c => c.Trim());
                query = query.Where(c => options.Contains(c.Technical.LiterRodziny));
            }
            if (Filter_KodWiazki != null)
            {
                var options = Filter_KodWiazki.Split(separator).Select(c => c.Trim());
                query = query.Where(c => options.Contains(c.Technical.KodWiazki));
            }
            if (Filter_NumerKompletu != null)
            {
                var options = Filter_NumerKompletu.Split(separator).Select(c => int.Parse(c.Trim()));
                query = query.Where(c => options.Contains(c.NumerKompletu));
            }
            if (Filter_SztukiZeskanowane != null)
            {
                var options = Filter_SztukiZeskanowane.Split(separator).Select(c => int.Parse(c.Trim()));
                query = query.Where(c => options.Contains(c.SztukiZeskanowane));
            }
            if (Filter_Wiazka != null)
            {
                var options = Filter_Wiazka.Split(separator).Select(c => c.Trim());
                foreach (var option in options)
                    query = query.Where(c => c.Wiazka.Contains(option));
            }
            if (Filter_KodCiety != null)
            {
                var options = Filter_KodCiety.Split(separator).Select(c => c.Trim());
                foreach (var option in options)
                    query = query.Where(c => c.KodCiety.Contains(option));
            }
            if (Filter_Pracownik != null)
            {
                var options = Filter_Pracownik.Split(separator).Select(c => c.Trim());
                foreach (var option in options)
                    query = query.Where(c => c.Pracownik.Contains(option));
            }
            if (Filter_DokDostawy != null)
            {
                var options = Filter_DokDostawy.Split(separator).Select(c => c.Trim());
                query = query.Where(c => options.Contains(c.DokDostawy));
            }
            if (Filter_DokDostawyDopis != null)
            {
                var options = Filter_DokDostawy.Split(separator).Select(c => c.Trim());
                query = query.Where(c => options.Contains(c.DostawaDopis));
            }

            if (Filter_DataUtworzeniaFrom != DateTime.MinValue && Filter_DataUtworzeniaTo != DateTime.MinValue)
                query = query.Where(c => c.DataUtworzenia.Date >= Filter_DataUtworzeniaFrom.Date && c.DataUtworzenia.Date <= Filter_DataUtworzeniaTo.Date);
            else if (Filter_DataUtworzeniaFrom != DateTime.MinValue)
                query = query.Where(c => c.DataUtworzenia.Date == Filter_DataUtworzeniaFrom);

            if(Filter_DataDostawyFrom != DateTime.MinValue && Filter_DataDostawyTo != DateTime.MinValue)
                query = query.Where(c => c.DataDostawy.Date >= Filter_DataDostawyFrom.Date && c.DataDostawy.Date <= Filter_DataDostawyTo.Date);
            else if (Filter_DataDostawyFrom != DateTime.MinValue)
                query = query.Where(c => c.DataDostawy.Date == Filter_DataDostawyFrom);

            if (Filter_DataDopisuFrom != DateTime.MinValue && Filter_DataDopisuTo != DateTime.MinValue)
                query = query.Where(c => c.DataDopisu != null && ((DateTime)c.DataDopisu).Date >= Filter_DataDopisuFrom && ((DateTime)c.DataDopisu).Date <= Filter_DataDopisuTo);
            else if (Filter_DataDopisuFrom != DateTime.MinValue)
                query = query.Where(c => c.DataDopisu != null && ((DateTime)c.DataDopisu).Date == Filter_DataDopisuFrom);

            if (Filter_Uwagi != null)
            {
                var options = Filter_Uwagi.Split(separator).Select(c => c.Trim());
                foreach (var option in options)
                    query = query.Where(c => c.Uwagi.Contains(option));
            }
            if (Filter_Komplet != null)
            {
                if(Filter_Komplet == "Prawda")
                    query = query.Where(c => c.Komplet == true);
                else if (Filter_Komplet == "Fałsz")
                    query = query.Where(c => c.Komplet == false);
            }
            if (Filter_Deklaracja != null)
            {
                if (Filter_Deklaracja == "Prawda")
                    query = query.Where(c => c.Deklarowany == true);
                else if (Filter_Deklaracja == "Fałsz")
                    query = query.Where(c => c.Deklarowany == false);
            }

            string[] sortings =
            {
                "Sztuki Zeskanowane",
                "Pracownik",
                "Dok. Dostawy",
                "Data Utworzenia",
                "Data Dostawy",
                "Data Dopisu",
                "Deklarowany",
                "Uwagi"
            };

            

            availableSortings = new SelectList(sortings);
            
            if (SortColumn != null)
            {
                switch(SortColumn)
                {
                    case "Sztuki Zeskanowane":
                        if (OrderDescent)
                            query = query.OrderByDescending(c => c.SztukiZeskanowane);
                        else
                            query = query.OrderBy(c => c.SztukiZeskanowane);
                        break;
                    case "Pracownik":
                        if (OrderDescent)
                            query = query.OrderByDescending(c => c.Pracownik);
                        else
                            query = query.OrderBy(c => c.Pracownik);
                        break;
                    case "Dok. Dostawy":
                        if (OrderDescent)
                            query = query.OrderByDescending(c => c.DokDostawy);
                        else
                            query = query.OrderBy(c => c.DokDostawy);
                        break;
                    case "Data Utworzenia":
                        if (OrderDescent)
                            query = query.OrderByDescending(c => c.DataUtworzenia);
                        else
                            query = query.OrderBy(c => c.DataUtworzenia);
                        break;
                    case "Data Dostawy":
                        if (OrderDescent)
                            query = query.OrderByDescending(c => c.DataDostawy);
                        else
                            query = query.OrderBy(c => c.DataDostawy);
                        break;
                    case "Data Dopisu":
                        if (OrderDescent)
                            query = query.OrderByDescending(c => c.DataDopisu);
                        else
                            query = query.OrderBy(c => c.DataDopisu);
                        break;
                    case "Deklarowany":
                        if (OrderDescent)
                            query = query.OrderByDescending(c => c.Deklarowany);
                        else
                            query = query.OrderBy(c => c.Deklarowany);
                        break;
                    case "Uwagi":
                        if (OrderDescent)
                            query = query.OrderByDescending(c => c.Uwagi);
                        else
                            query = query.OrderBy(c => c.Uwagi);
                        break;
                }
            }
            int count = query.Count();
            Data = await PaginatedList<Data.Tables.VTMagazyn>.CreateAsync(query, CurrentPage, PageSize);
            return;
        }


        public async Task<IActionResult> OnPostPageDownloadCsvAsync()
        {
            await OnGetAsync();
            MemoryStream ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.AutoFlush = true;
            var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(Data);
            await ms.FlushAsync();
            await csv.FlushAsync();
            ms.Position = 0;

            return File(ms, "text/csv", $"vtmagazyn-strona-{Data.PageIndex}.csv");
        }
        public async Task<IActionResult> OnPostDeleteRecordAsync(Guid id)
        {
            var rec = await _db.VTMagazyn.FindAsync(id);
            if (rec != null)
            {
                if (rec.SztukiZeskanowane >= rec.SztukiDeklarowane)
                {
                    var toUpdate = _db.VTMagazyn.Where(c =>
                        c.Wiazka == rec.Wiazka &&
                        c.NumerKompletu == rec.NumerKompletu &&
                        c.DataDostawy.Date == rec.DataDostawy.Date).ToList();
                    foreach (var record in toUpdate)
                    {
                        record.ZeskanowanychNaKomplet--;
                        if (record.autocompleteEnabled)
                            record.Komplet = false;
                    }

                    _db.UpdateRange(toUpdate);
                }
                _db.VtToDostawa.RemoveRange(_db.VtToDostawa.Where(c => c.VTMagazynId == id));
                _db.VTMagazyn.Remove(rec);
                await _db.SaveChangesAsync();
            }

            await OnGetAsync();

            return Page();
        }
    }
}