using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ZEM_Enterprice_WebApp.Data;
using ZEM_Enterprice_WebApp.Data.Tables;

namespace ZEM_Enterprice_WebApp.Pages.Department.Office
{
    [Authorize(Policy = "CanViewVTScans")]
    public class DifferenceReportModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public DateTime ForDate { get; set; }
        public Dictionary<string, Differences> differences { get; set; }
        public Dictionary<string, Differences> differencesFiltered { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Filtruj nadmiary")]
        public bool Filter_Nadmiary { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Filtruj niedomiary")]
        public bool Filter_Niedomiary { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Filtruj braki")]
        public bool Filter_Braki { get; set; }

        public DifferenceReportModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task OnGet(int day, int month, int year)
        {
            ForDate = new DateTime(year, month, day);
            CreateDifferences();
            return;
            //return File(Encoding.UTF8.GetBytes(CreateFile().ToString()), "text/plain", "raport.csv");
        }

        public void CreateDifferences()
        {
            differences = new Dictionary<string, Differences>();
            List<TempDostawa> dostawaForDay = _db.Dostawa.IgnoreQueryFilters().AsNoTracking().Where(c => c.Data.Date == ForDate.Date)
                .Include(c => c.Technical)
                .Include(c => c.Skany)
                .Where(c => !c.Technical.KanBan)
                .Select(c => new TempDostawa
                {
                    Data = c.Data,
                    Kod = c.Kod.Replace("PLC", "").TrimStart('0'),
                    Ilosc = c.Ilosc,
                    Uwagi = c.Uwagi,
                    Technical = c.Technical,
                    Skany = c.Skany.Select(c => c.VTMagazyn).ToList()
                }).ToList();


            foreach (var dostawaEntry in dostawaForDay)
            {
                List<VTMagazyn> vtEntries = new List<VTMagazyn>();
                //List<VTMagazyn> vtEntries2 = _db.VTMagazyn.AsNoTracking().Where(c => c.KodCiety == dostawaEntry.Kod && c.DataDostawy.Date == ForDate.Date || (c.DataDopisu != null && ((DateTime)c.DataDopisu).Date == ForDate.Date)).Include(c => c.Dostawy).ToList();
                if(dostawaEntry.Skany != null)
                    foreach (var skan in dostawaEntry.Skany)
                        vtEntries.Add(skan);

                string BIN = dostawaEntry.Technical.BIN;

                int declaredCount = dostawaEntry.Ilosc;
                int sztukiSkanowane = 0;
                string wiazka;
                string kod;

                if (vtEntries.Count() > 0)
                {
                    foreach (var vtEntry in vtEntries)
                    {
                        if (vtEntry.DataDopisu != null && ((DateTime)vtEntry.DataDopisu).Date == ForDate.Date)
                            sztukiSkanowane += vtEntry.DopisanaIlosc;
                        else if (vtEntry.DataDopisu != null && vtEntry.DataDostawy.Date == ForDate.Date)
                            sztukiSkanowane += vtEntry.SztukiZeskanowane - vtEntry.DopisanaIlosc;
                        else if (vtEntry.DataDostawy.Date == ForDate.Date)
                            sztukiSkanowane += vtEntry.SztukiZeskanowane;
                        else
                            sztukiSkanowane += 0;

                        //if (vtEntry.DataDopisu != null)
                        //    sztukiSkanowane += vtEntry.DopisanaIlosc;
                        //else
                        //    sztukiSkanowane += vtEntry.SztukiZeskanowane;
                    }

                    wiazka = vtEntries[0].Wiazka;
                    kod = vtEntries[0].KodCiety;
                }
                else
                {
                    kod = dostawaEntry.Kod;
                    sztukiSkanowane = 0;
                    wiazka = dostawaEntry.Technical.Wiazka;
                }


                if (differences.ContainsKey(kod))
                {
                    differences[kod].Zeskanowanych += sztukiSkanowane;
                }
                else
                {
                    differences.Add(kod, new Differences
                    {
                        Wiazka = wiazka,
                        BIN = BIN,
                        Zeskanowanych = sztukiSkanowane,
                        Deklarowanych = declaredCount
                    });
                }
            }

            List<VTMagazyn> vtUndeclared = _db.VTMagazyn.IgnoreQueryFilters().AsNoTracking().Include(c=>c.Dostawy)
                .Where(c => 
                    (c.Deklarowany == false || c.Dostawy.Count() == 0) && 
                    (c.DataDostawy.Date == ForDate.Date ||
                    (c.DataDopisu != null && ((DateTime)c.DataDopisu).Date == ForDate.Date)))
                .Include(c=>c.Technical).ToList();

            foreach (var vtEntry in vtUndeclared)
            {
                //if (vtEntry.DataDopisu == null && vtEntry.DataDostawy.Date != ForDate.Date)
                //    break;

                int sztukiSkanowane;

                if (vtEntry.Deklarowany == true && vtEntry.DataDostawy.Date == ForDate.Date)
                    continue;
                else if (vtEntry.Deklarowany == false && vtEntry.Dostawy.Count() > 0 && vtEntry.DataDopisu != null && ((DateTime)vtEntry.DataDopisu).Date == ForDate.Date)
                    continue;
                else if (vtEntry.DataDopisu != null && ((DateTime)vtEntry.DataDopisu).Date == ForDate.Date)
                    sztukiSkanowane = vtEntry.DopisanaIlosc;
                else if (vtEntry.DataDopisu != null && vtEntry.DataDostawy.Date == ForDate.Date)
                    sztukiSkanowane = vtEntry.SztukiZeskanowane - vtEntry.DopisanaIlosc;
                else if (vtEntry.DataDostawy.Date == ForDate.Date)
                    sztukiSkanowane = vtEntry.SztukiZeskanowane;
                else
                    sztukiSkanowane = 0;

                if (differences.ContainsKey(vtEntry.KodCiety))
                {
                    differences[vtEntry.KodCiety].Zeskanowanych += vtEntry.SztukiZeskanowane;
                }
                else
                {
                    differences.Add(vtEntry.KodCiety, new Differences
                    {
                        Wiazka = vtEntry.Wiazka,
                        BIN = vtEntry.Technical.BIN,
                        Zeskanowanych = sztukiSkanowane,
                        Deklarowanych = 0
                    });
                }
            }

            var notDeclaredAddedToDeclared = _db.VTMagazyn.IgnoreQueryFilters().AsNoTracking().Include(c => c.Dostawy)
                .Where(c => 
                    c.DataDopisu != null && ((DateTime)c.DataDopisu).Date == ForDate.Date &&
                    !dostawaForDay.Select(c => c.Kod.Replace("PLC", "")).Contains(c.KodCiety)
                )
                .Include(c => c.Technical).ToList();

            foreach(var vtEntry in notDeclaredAddedToDeclared)
            {
                if (differences.ContainsKey(vtEntry.KodCiety))
                {
                    differences[vtEntry.KodCiety].Zeskanowanych += vtEntry.SztukiZeskanowane;
                }
                else
                {
                    differences.Add(vtEntry.KodCiety, new Differences
                    {
                        Wiazka = vtEntry.Wiazka,
                        BIN = vtEntry.Technical.BIN,
                        Zeskanowanych = vtEntry.DopisanaIlosc,
                        Deklarowanych = 0
                    });
                }
            }

            if (Filter_Braki)
                differencesFiltered = differences.Where(c => c.Value.Zeskanowanych == 0).ToDictionary(c => c.Key, c => c.Value);
            else if (Filter_Niedomiary)
                differencesFiltered = differences.Where(c => c.Value.Zeskanowanych - c.Value.Deklarowanych < 0 && c.Value.Zeskanowanych != 0).ToDictionary(c => c.Key, c => c.Value);
            else if (Filter_Nadmiary)
                differencesFiltered = differences.Where(c => c.Value.Zeskanowanych - c.Value.Deklarowanych > 0).ToDictionary(c => c.Key, c => c.Value);
            else
                differencesFiltered = differences;
        }
        public StringBuilder CreateFile()
        {
            StringBuilder writer = new StringBuilder();
            writer.AppendLine($"Data utworzenia raportu: {ForDate.ToString("d", CultureInfo.CreateSpecificCulture("de-DE"))}");
            writer.AppendLine("Wiazka,Kod Ciety,BIN,Zeskanowane,Deklarowane,Roznica,Uwagi");
            foreach (KeyValuePair<string, Differences> entry in differencesFiltered)
            {
                if (entry.Value.Zeskanowanych - entry.Value.Deklarowanych != 0)
                {
                    string line = $"{entry.Value.Wiazka},{entry.Key},{entry.Value.BIN},{entry.Value.Zeskanowanych},{entry.Value.Deklarowanych},{entry.Value.Zeskanowanych - entry.Value.Deklarowanych}";

                    if (entry.Value.Zeskanowanych == 0)
                    {
                        writer.AppendLine($"{line},brak");
                    }
                    else if (entry.Value.Zeskanowanych - entry.Value.Deklarowanych < 0)
                    {
                        writer.AppendLine($"{line},niedomiar");
                    }
                    else
                    {
                        writer.AppendLine($"{line},nadmiar");
                    }
                }
            }

            return writer;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            CreateDifferences();
            return File(Encoding.UTF8.GetBytes(CreateFile().ToString()), "text/plain", $"raport-{ForDate.Day}-{ForDate.Month}-{ForDate.Year}.csv");
        }

    }


    public class Differences
    {
        public string Wiazka { get; set; }
        public string BIN { get; set; }
        public int Zeskanowanych { get; set; }
        public int Deklarowanych { get; set; }
    }

    public class TempDostawa
    {
        public DateTime Data { get; set; }
        public string Kod { get; set; }
        public int Ilosc { get; set; }
        public string Uwagi { get; set; }
        public Data.Tables.Technical Technical { get; set; }
        public List<VTMagazyn> Skany { get; set; }
    }

}