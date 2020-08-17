using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ZEM_Enterprice_WebApp.Data;
using ZEM_Enterprice_WebApp.Data.Tables;
using ZEM_Enterprice_WebApp.Utilities;

namespace ZEM_Enterprice_WebApp.Pages.Department.Technical
{
    [Authorize(Policy = "AdminOrTech")]
    public class UploadFileModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        [Required]
        public IFormFile dataFile { get; set; }


        public UploadFileModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            List<PendingChangesTechnical> duplicates = new List<PendingChangesTechnical>();
            List<Data.Tables.Technical> TechEntries = new List<Data.Tables.Technical>();
            List<Data.Tables.Technical> dups = new List<Data.Tables.Technical>();

            using (var sr = new StreamReader(dataFile.OpenReadStream()))
            {
                while (sr.Peek() >= 0)
                {
                    string[] fields = ((await sr.ReadLineAsync())).Split(',');
                    bool kanban = false;
                    if (fields[8] != "")
                        kanban = true;
                    string primaryKey = fields[1] + "_" + fields[5];

                    TechEntries.Add(new Data.Tables.Technical
                    {
                        CietyWiazka = primaryKey,
                        Rodzina = fields[0],
                        Wiazka = fields[1],
                        LiterRodziny = fields[2],
                        KodWiazki = fields[3],
                        IlePrzewodow = fields[4],
                        PrzewodCiety = fields[5],
                        BIN = fields[6],
                        IndeksScala = fields[7],
                        KanBan = kanban,
                        Uwagi = "",
                        DataUtworzenia = fields[9]
                    });
                }
            }
            dups.AddRange(TechEntries.GroupBy(c => c.CietyWiazka).Where(g => g.Count() > 1).Select(c => c.ToList()).Select(c =>c[0]).ToList());
            duplicates.AddRange(TechEntries.GroupBy(c => c.CietyWiazka).Where(g => g.Count() > 1).Select(c => c.ToList()).Select(
                c => new PendingChangesTechnical
                {
                    CietyWiazka = c[0].CietyWiazka,
                    Rodzina = c[0].Rodzina,
                    Wiazka = c[0].Wiazka,
                    LiterRodziny = c[0].LiterRodziny,
                    KodWiazki = c[0].KodWiazki,
                    IlePrzewodow = c[0].IlePrzewodow,
                    PrzewodCiety = c[0].PrzewodCiety,
                    BIN = c[0].BIN,
                    IndeksScala = c[0].IndeksScala,
                    KanBan = c[0].KanBan,
                    Uwagi = "",
                    DataUtworzenia = c[0].DataUtworzenia,
                    DataModyfikacji = DateTime.Now.ToString("g", CultureInfo.CreateSpecificCulture("de-DE"))
                }).ToList());
            foreach(var dup in dups)
                TechEntries.Remove(dup);
            TechEntries = TechEntries.GroupBy(c => c.CietyWiazka).Distinct().Select(c => c.ToList()[0]).ToList();

            duplicates.AddRange(TechEntries.Where(d => _db.Technical.IgnoreQueryFilters().Select(c => c.CietyWiazka).Contains(d.CietyWiazka))
                .Select(c => new PendingChangesTechnical
                {
                    CietyWiazka = c.CietyWiazka,
                    Rodzina = c.Rodzina,
                    Wiazka = c.Wiazka,
                    LiterRodziny = c.LiterRodziny,
                    KodWiazki = c.KodWiazki,
                    IlePrzewodow = c.IlePrzewodow,
                    PrzewodCiety = c.PrzewodCiety,
                    BIN = c.BIN,
                    IndeksScala = c.IndeksScala,
                    KanBan = c.KanBan,
                    Uwagi = "",
                    DataUtworzenia = c.DataUtworzenia,
                    DataModyfikacji = DateTime.Now.ToString("g", CultureInfo.CreateSpecificCulture("de-DE"))
                }).ToList());

            await _db.PendingChangesTechnical.AddRangeAsync(duplicates);
            await _db.Technical.AddRangeAsync(TechEntries.Where(c => !_db.Technical.IgnoreQueryFilters().Select(d => d.CietyWiazka).Contains(c.CietyWiazka)));

            await _db.SaveChangesAsync();

            if (duplicates.Count > 0)
                return RedirectToPage("/Department/Technical/ConfirmChanges");
            else
                return RedirectToPage("/MainPage");
        }
    }
}