using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ZEM_Enterprice_WebApp.Data;
using ZEM_Enterprice_WebApp.Data.Tables;

namespace ZEM_Enterprice_WebApp.Pages.Department.Technical
{
    [Authorize(Policy = "AdminOrTech")]
    public class ConfirmChangesModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public List<Data.Tables.PendingChangesTechnical> technicalDuplicates { get; set; }

        public List<Input> _input { get; set;}

        [BindProperty]
        public List<string> AreChecked { get; set; }
        

        public ConfirmChangesModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            _input = new List<Input>();

            technicalDuplicates = _db.PendingChangesTechnical.ToList();
            foreach(var dub in technicalDuplicates)
            {
                _input.Add(new Input
                {
                    Id = dub.PendingChangesTechnicalId,
                    DataModyfikacji = dub.DataModyfikacji,
                    BIN = dub.BIN,
                    CietyWiazka = dub.CietyWiazka,
                    IlePrzewodow = dub.IlePrzewodow,
                    IndeksScala = dub.IndeksScala,
                    KanBan = dub.KanBan,
                    KodWiazki = dub.KodWiazki,
                    LiterRodziny = dub.LiterRodziny,
                    PrzewodCiety = dub.PrzewodCiety,
                    Rodzina = dub.Rodzina,
                    Uwagi = dub.Uwagi,
                    Wiazka = dub.Wiazka,
                    selected = false
                });
            }
        }

        public async Task<IActionResult> OnPostDownloadCsvAsync()
        {
            StringBuilder sb = new StringBuilder();
            MemoryStream ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(await _db.PendingChangesTechnical.ToListAsync());
            await csv.FlushAsync();
            ms.Position = 0;

            return File(ms, "text/csv", "powtorzenia.csv");
        }

        public async Task<IActionResult> OnPostAcceptChangesAsync()
        {
            foreach (var check in AreChecked)
            {
                var pending = await _db.PendingChangesTechnical.FirstOrDefaultAsync(c => c.PendingChangesTechnicalId == Guid.Parse(check));
                var rec = await _db.Technical.FirstOrDefaultAsync(c => c.CietyWiazka == pending.CietyWiazka);

                if(rec != null)
                {
                    rec.BIN = pending.BIN;
                    rec.IlePrzewodow = pending.IlePrzewodow;
                    rec.IndeksScala = pending.IndeksScala;
                    rec.KanBan = pending.KanBan;
                    rec.KodWiazki = pending.KodWiazki;
                    rec.LiterRodziny = pending.LiterRodziny;
                    rec.PrzewodCiety = pending.PrzewodCiety;
                    rec.Rodzina = pending.Rodzina;
                    rec.Uwagi = pending.Uwagi;
                    rec.Wiazka = pending.Wiazka;
                    rec.DeleteDate = null;
                    rec.Deleted = false;
                }
                _db.Technical.Update(rec);
            }
            _db.PendingChangesTechnical.RemoveRange(_db.PendingChangesTechnical);
            await _db.SaveChangesAsync();
            return RedirectToPage("/MainPage");
        }
    }
    public class Input
    {
        public Guid Id { get; set; }
        public bool selected { get; set; }
        public string CietyWiazka { get; set; }
        public string Rodzina { get; set; }
        public string Wiazka { get; set; }
        public string LiterRodziny { get; set; }
        public string KodWiazki { get; set; }
        public string IlePrzewodow { get; set; }
        public string PrzewodCiety { get; set; }
        public string BIN { get; set; }
        public string IndeksScala { get; set; }
        public bool KanBan { get; set; }
        public string Uwagi { get; set; }
        public string DataModyfikacji { get; set; }
    }
}