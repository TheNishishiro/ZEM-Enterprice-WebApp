using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ZEM_Enterprice_WebApp.Data;
using ZEM_Enterprice_WebApp.Utilities;

namespace ZEM_Enterprice_WebApp.Pages.Department.Office
{
    [Authorize(Policy = "CanViewSupply")]
    public class ConfirmMissingModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public List<Data.Tables.PendingDostawa> pendingDostawas { get; set; }

        public class Input
        {
            public Guid dostawaId { get; set; }
            public string Kod { get; set; }
            public int Ilosc { get; set; }
            public DateTime Data { get; set; }
            public string Uwagi { get; set; }
            public bool selected { get; set; }
        }

        [BindProperty]
        public List<Input> _input { get; set; }

        [BindProperty]
        public List<string> AreChecked { get; set; }


        public ConfirmMissingModel(ApplicationDbContext db)
        {
            _db = db;
        }
        


        public async Task OnGetAsync()
        {
            pendingDostawas = _db.PendingDostawa.ToList();
            _input = new List<Input>();

            foreach(var pendingDostawa in pendingDostawas)
            {
                _input.Add(new Input
                {
                    dostawaId = pendingDostawa.PendingDostawaId,
                    Kod = pendingDostawa.Kod,
                    Uwagi = pendingDostawa.Uwagi,
                    Data = pendingDostawa.Data,
                    Ilosc = pendingDostawa.Ilosc,
                    selected = false
                });
            }
            return;
        }

        public async Task<IActionResult> OnPostAcceptChangesAsync()
        {
            foreach (var check in AreChecked)
            {
                var pending = await _db.PendingDostawa.FirstOrDefaultAsync(c => c.PendingDostawaId == Guid.Parse(check));
                var tech = await _db.Technical.FirstOrDefaultAsync(c => c.IndeksScala == pending.Kod);

                if (tech != null)
                {
                    await _db.Dostawa.AddAsync(new Data.Tables.Dostawa
                    {
                        Kod = pending.Kod,
                        Ilosc = pending.Ilosc,
                        Data = pending.Data,
                        DataUtworzenia = DateTime.Now,
                        Uwagi = "",
                        Technical = tech
                    });
                    _db.Remove(pending);
                }
            }
            await _db.SaveChangesAsync();
            return RedirectToPage("/MainPage");
        }
        public async Task<IActionResult> OnPostDownloadCsvAsync()
        {
            return File(await CSVDownloader.OnPostDownloadCsvAsync(_db, _db.PendingDostawa.AsQueryable()), "text/csv", $"braki-z-dostawy.csv");
        }

        public async Task<IActionResult> OnPostDeleteRecordAsync(string id)
        {
            var pending = await _db.PendingDostawa.FirstOrDefaultAsync(c => c.PendingDostawaId == Guid.Parse(id));
            if (pending == null)
                return Page();

            _db.Remove(pending);
            await _db.SaveChangesAsync();

            return Page();
        }
    }

    
}