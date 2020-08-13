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
using Microsoft.EntityFrameworkCore;
using ZEM_Enterprice_WebApp.Data;

namespace ZEM_Enterprice_WebApp.Pages.Manager.AdminManager
{
    [Authorize(Roles = "Admin")]
    public class SupplyArchiveModel : PageModel
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

        public SupplyArchiveModel(ApplicationDbContext db)
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

            string fileName = $"DOSTAWA_Archive-{Date.Day}-{Date.Month}-{Date.Year}.csv";

            var query = _db.Dostawa.Where(c => c.Data.Date <= Date.Date).AsQueryable();

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

                    foreach (var record in records)
                    {
                        _db.VtToDostawa.RemoveRange(_db.VtToDostawa.Where(c => c.DostawaId == record.DostawaId));
                    }
                }
            }

            await _db.SaveChangesAsync();

            _db.RemoveRange(query);

            await _db.SaveChangesAsync();

            StatusHeader = "Sukces";
            StatusMessage = "Rekordy dostawy zostały zapisane do pliku.";

            return Page();
        }
    }
}