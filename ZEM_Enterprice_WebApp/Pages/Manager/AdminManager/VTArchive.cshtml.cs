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
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;
using ZEM_Enterprice_WebApp.Data;

namespace ZEM_Enterprice_WebApp.Pages.Manager.AdminManager
{
    [Authorize(Roles = "Admin")]
    public class VTArchiveModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [Required]
        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [TempData]
        public string StatusHeader { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public VTArchiveModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task OnGetAsync()
        {
            Date = DateTime.Now.AddMonths(-3);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (!Directory.Exists("./Archiwum"))
                Directory.CreateDirectory("./Archiwum");

            string fileName = $"VT_Archive-{Date.Day}-{Date.Month}-{Date.Year}.csv";

            var query = _db.VTMagazyn.Where(c => c.DataUtworzenia.Date <= Date.Date).AsQueryable();

            if (!System.IO.File.Exists($"./Archiwum/{fileName}"))
            {
                int recordsInTable = query.Count();
                int chunkSize = 20000;
                for (int i = 0; i < recordsInTable; i += chunkSize)
                {
                    var records = await query.Skip(i).Take(chunkSize).ToListAsync();

                    using (var writer = new StreamWriter($"./Archiwum/{fileName}", true))
                    {
                        writer.AutoFlush = true;
                        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                        {
                            if (i != 0)
                                csv.Configuration.HasHeaderRecord = false;
                            csv.WriteRecords(records);
                            await csv.FlushAsync();
                        }
                    }
                }

                for (int i = 0; i < recordsInTable; i += chunkSize)
                {
                    var records = await query.Skip(i).Take(chunkSize).ToListAsync();

                    foreach(var record in records)
                    {
                        _db.VtToDostawa.RemoveRange(_db.VtToDostawa.Where(c => c.VTMagazynId == record.VTMagazynId));
                    }
                }
            }

            await _db.SaveChangesAsync();

            _db.RemoveRange(query);

            await _db.SaveChangesAsync();

            StatusHeader = "Sukces";
            StatusMessage = "Rekordy VT zostały zapisane do pliku.";

            return Page();
        }
    }
}