using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using ZEM_Enterprice_WebApp.Data;
using ZEM_Enterprice_WebApp.Data.Tables;

namespace ZEM_Enterprice_WebApp.Pages.Department.Office
{
    [Authorize(Policy = "CanDoAnal")]
    public class AnalizeReportModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public DateTime ForDateStart { get; set; }
        [BindProperty]
        public DateTime ForDateEnd { get; set; }
        public List<AnalizeEntry> analizeEntries { get; set; }
        public List<AnalizeEntry> analizeEntriesFiltered { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Filtruj kompletne")]
        public bool Filter_Complete { get; set; }
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Filtruj nie kompletne")]
        public bool Filter_NotComplete { get; set; }


        public AnalizeReportModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task OnGet(int day, int month, int year)
        {
            ForDateStart = new DateTime(year, month, day);
            CreateReportImproved();
            return;
        }

        public void CreateReportImproved()
        {
            List<VTMagazyn> VTEntries = _db.VTMagazyn.IgnoreQueryFilters().AsNoTracking().Where(c =>
                   c.DataDostawy.Date == ForDateStart.Date).Include(c => c.Technical).ToList();

            List<Dostawa> DostawaEntries = _db.Dostawa.AsNoTracking().Where(c =>
                   c.Data.Date == ForDateStart.Date).IgnoreQueryFilters().Include(c => c.Technical).Where(c => c.Technical.KanBan == false).ToList();

            analizeEntries = new List<AnalizeEntry>();
            foreach(VTMagazyn vt in VTEntries)
            {
                var entry = analizeEntries.FirstOrDefault(c => c.KodCiety == vt.KodCiety && c.Wiazka == vt.Wiazka && c.NrKompletu == vt.NumerKompletu);
                if (entry == null)
                {
                    analizeEntries.Add(new AnalizeEntry
                    {
                        KodCiety = vt.KodCiety,
                        Wiazka = vt.Wiazka,
                        Komplet = int.Parse(vt.Technical.IlePrzewodow),
                        Rodzina = vt.Technical.Rodzina,
                        Status = false,
                        Suma = vt.SztukiZeskanowane,
                        isCompleted = vt.Komplet,
                        NrKompletu = vt.NumerKompletu,
                        DeclaredValue = vt.SztukiDeklarowane
                    });
                }
                else
                {
                    entry.Suma += vt.SztukiZeskanowane;
                }
            }

            foreach (Dostawa dostawa in DostawaEntries)
            {
                var entry = analizeEntries.FirstOrDefault(
                    c => c.KodCiety == dostawa.Technical.PrzewodCiety && c.Wiazka == dostawa.Technical.Wiazka && c.NrKompletu == 0);
                if (entry == null)
                {
                    analizeEntries.Add(new AnalizeEntry
                    {
                        KodCiety = dostawa.Technical.PrzewodCiety,
                        Wiazka = dostawa.Technical.Wiazka,
                        Komplet = int.Parse(dostawa.Technical.IlePrzewodow),
                        Rodzina = dostawa.Technical.Rodzina,
                        Status = false,
                        Suma = 0,
                        isCompleted = false,
                        NrKompletu = 0,
                        DeclaredValue = dostawa.Ilosc
                    });
                }
            }

            List<Data.Tables.Technical> TechnicalEntries = _db.Technical.Where(
                c =>
                (VTEntries.Select(c => c.Wiazka).Contains(c.Wiazka) //&&
                //!VTEntries.Select(c => c.KodCiety).Contains(c.PrzewodCiety))
                ||
                //VTEntries.Select(c => c.Wiazka).Contains(c.Wiazka) &&
                DostawaEntries.Select(c => c.Technical.Wiazka).Contains(c.Wiazka)) &&
                //!DostawaEntries.Select(c => c.Kod).Contains(c.IndeksScala))) &&
                //!VTEntries.Select(c => c.KodCiety).Contains(c.PrzewodCiety) &&
                c.KanBan == false
                ).ToList();

            foreach (var technical in TechnicalEntries)
            {
                int sets = analizeEntries.Where(c => c.Wiazka == technical.Wiazka).Select(c => c.NrKompletu).Max();

                for (int i = 0; i <= sets; i++)
                {
                    var entry = analizeEntries.FirstOrDefault(c => c.KodCiety == technical.PrzewodCiety && c.Wiazka == technical.Wiazka && c.NrKompletu == i);

                    if (entry == null)
                    {
                        analizeEntries.Add(new AnalizeEntry
                        {
                            KodCiety = technical.PrzewodCiety,
                            Wiazka = technical.Wiazka,
                            Komplet = int.Parse(technical.IlePrzewodow),
                            Rodzina = technical.Rodzina,
                            Status = false,
                            Suma = 0,
                            isCompleted = false,
                            NrKompletu = i,
                            DeclaredValue = 0
                        });
                    }
                }
            }

            analizeEntries = analizeEntries.OrderBy(c => c.Rodzina).ThenBy(c => c.Wiazka).ThenBy(c => c.NrKompletu).ThenBy(c => c.KodCiety).ToList();
            var grouped = analizeEntries.GroupBy(c => new { c.Wiazka, c.NrKompletu }).Select(g => g.ToList()).ToList();
            foreach (var entryPerWiazka in grouped)
            {
                //var distincValues = entryPerWiazka.Select(c => c.Suma).Distinct().ToList();
                //if ((distincValues.Count() == 1 && distincValues[0] != 0) || (entryPerWiazka.Select(c => c.isCompleted).Contains(true)))
                //{
                //    foreach (var entry in entryPerWiazka)
                //        entry.Status = true;
                //}

                var markedToComplete = entryPerWiazka.Select(c => c.Suma != c.DeclaredValue).Count();
                if(markedToComplete == 0 || entryPerWiazka.Select(c => c.isCompleted).Contains(true))
                    foreach (var entry in entryPerWiazka)
                        entry.Status = true;
            }

            if (Filter_Complete)
                analizeEntriesFiltered = analizeEntries.Where(c => c.Status == true).ToList();
            else if (Filter_NotComplete)
                analizeEntriesFiltered = analizeEntries.Where(c => c.Status == false).ToList();
            else
                analizeEntriesFiltered = analizeEntries;

            string prevWiazka = "", prevRodzina = "", cutCode = "";
            int setID = 0;
            foreach (var entry in analizeEntriesFiltered)
            {
                if (prevWiazka != entry.Wiazka)
                {
                    prevWiazka = entry.Wiazka;
                    entry.NextWiazka = true;

                    entry.NewSet = true;
                    setID = entry.NrKompletu;
                }
                if(setID != entry.NrKompletu)
                {
                    entry.NewSet = true;
                    setID = entry.NrKompletu;
                }
                if (prevRodzina != entry.Rodzina)
                {
                    prevRodzina = entry.Rodzina;
                    entry.NextRodzina = true;
                }
            }
        }

        public StringBuilder CreateFile()
        {
            StringBuilder writer = new StringBuilder();
            writer.AppendLine($"Data utworzenia raportu: {ForDateStart.ToString("d", CultureInfo.CreateSpecificCulture("de-DE"))}");
            writer.AppendLine("Rodzina,Wiazka,Kpl,Kod ciety,Ilosc,Status");
            foreach (var entry in analizeEntriesFiltered)
            {
                string line = $"{entry.Rodzina},{entry.Wiazka},{entry.Komplet},{entry.KodCiety},{entry.Suma}";

                if (entry.Status)
                {
                    writer.AppendLine($"{line},komplet");
                }
                else
                {
                    writer.AppendLine($"{line},brak kompletu");
                }
            }

            return writer;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            CreateReportImproved();
            return File(Encoding.UTF8.GetBytes(CreateFile().ToString()), "text/plain", $"analiza-{ForDateStart.Day}-{ForDateStart.Month}-{ForDateStart.Year}.csv");
        }
    }

    public class AnalizeEntry
    {
        public string Rodzina { get; set; }
        public string Wiazka { get; set; }
        public int Komplet { get; set; }
        public int NrKompletu { get; set; }
        public bool isCompleted { get; set; }
        public string KodCiety { get; set; }
        public int Suma { get; set; }
        public int DeclaredValue { get; set; }
        public bool Status { get; set; }
        public bool NextWiazka { get; set; }
        public bool NextRodzina { get; set; }
        public bool NewSet { get; set; }
    }
}