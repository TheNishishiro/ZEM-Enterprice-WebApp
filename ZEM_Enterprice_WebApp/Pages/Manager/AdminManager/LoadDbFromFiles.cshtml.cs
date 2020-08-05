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
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;
using ZEM_Enterprice_WebApp.Data;
using ZEM_Enterprice_WebApp.Data.Tables;

namespace ZEM_Enterprice_WebApp.Pages.Manager.AdminManager
{
    [Authorize(Roles = "Admin")]
    public class LoadDbFromFilesModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [Required]
        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public LoadDbFromFilesModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task OnGetAsync()
        {
            Date = DateTime.Now;
        }

        public async Task<IActionResult> OnPostDeleteDbAsync()
        {
            if (!Directory.Exists("./Snapshots"))
                return Page();
            string backupDir = $"./Snapshots/{Date.Day}{Date.Month}{Date.Year}";
            if (!Directory.Exists(backupDir))
                return Page();

            _db.RemoveRange(_db.VtToDostawa);
            _db.RemoveRange(_db.Dostawa);
            _db.RemoveRange(_db.VTMagazyn);
            _db.RemoveRange(_db.Technical);

            await _db.SaveChangesAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostLoadTechnicalAsync()
        {
            int chunkSize = 20000;
            string backupDir = $"./Snapshots/{Date.Day}{Date.Month}{Date.Year}";
            if (!Directory.Exists("./Snapshots"))
                return Page();

            if (!Directory.Exists(backupDir))
                return Page();

            if(System.IO.File.Exists(backupDir + "/Technical.csv"))
            {
                using (var reader = new StreamReader(backupDir + "/Technical.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    _db.RemoveRange(_db.Technical);
                    await _db.SaveChangesAsync();
                    int i = 0;
                    await csv.ReadAsync();
                    csv.ReadHeader();
                    while(await csv.ReadAsync())
                    {
                        await _db.Technical.AddAsync(csv.GetRecord<Technical>());
                        i++;

                        if (i >= chunkSize)
                        {
                            await _db.SaveChangesAsync();
                            i = 0;
                        }
                    }
                }
            }

            await _db.SaveChangesAsync();
            return RedirectToPage("/Manager/AdminManager/LoadDbFromFiles");
        }

        public async Task<IActionResult> OnPostLoadDostawyAsync()
        {
            int chunkSize = 20000;
            string backupDir = $"./Snapshots/{Date.Day}{Date.Month}{Date.Year}";
            if (!Directory.Exists("./Snapshots"))
                return Page();

            if (!Directory.Exists(backupDir))
                return Page();

            _db.RemoveRange(_db.VtToDostawa);

            await _db.SaveChangesAsync();

            if (System.IO.File.Exists(backupDir + "/Dostawa.csv"))
            {
                using (var reader = new StreamReader(backupDir + "/Dostawa.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    _db.RemoveRange(_db.Dostawa);
                    int i = 0;
                    await csv.ReadAsync();
                    csv.ReadHeader();
                    while (await csv.ReadAsync())
                    {
                        var record = new Dostawa
                        {
                            Data = csv.GetField<DateTime>("Data"),
                            Ilosc = csv.GetField<int>("Ilosc"),
                            Kod = csv.GetField<string>("Kod"),
                            KodIloscData = csv.GetField<string>("KodIloscData"),
                            Skany = new List<VtToDostawa>(),
                            Technical = await _db.Technical.FindAsync(csv.GetField<string>("CietyWiazka")),
                            Uwagi = csv.GetField<string>("Uwagi")

                        };
                        await _db.Dostawa.AddAsync(record);
                        i++;

                        if (i >= chunkSize)
                        {
                            await _db.SaveChangesAsync();
                            i = 0;
                        }
                    }
                }
            }
            await _db.SaveChangesAsync();
            return RedirectToPage("/Manager/AdminManager/LoadDbFromFiles");
        }

        public async Task<IActionResult> OnPostLoadVTMagazynAsync()
        {
            int chunkSize = 20000;
            string backupDir = $"./Snapshots/{Date.Day}{Date.Month}{Date.Year}";
            if (!Directory.Exists("./Snapshots"))
                return Page();

            if (!Directory.Exists(backupDir))
                return Page();

            
            if (System.IO.File.Exists(backupDir + "/VTMagazyn.csv"))
            {
                using (var reader = new StreamReader(backupDir + "/VTMagazyn.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    _db.RemoveRange(_db.VTMagazyn);
                    int i = 0;
                    await csv.ReadAsync();
                    csv.ReadHeader();
                    while (await csv.ReadAsync())
                    {
                        var record = new VTMagazyn
                        {
                            VTMagazynId = csv.GetField<Guid>("VTMagazynId"),
                            NumerKompletu = csv.GetField<int>("NumerKompletu"),
                            SztukiDeklarowane = csv.GetField<int>("SztukiDeklarowane"),
                            SztukiZeskanowane = csv.GetField<int>("SztukiZeskanowane"),
                            ZeskanowanychNaKomplet = csv.GetField<int>("ZeskanowanychNaKomplet"),
                            NaKomplet = csv.GetField<int>("NaKomplet"),
                            Wiazka = csv.GetField<string>("Wiazka"),
                            KodCiety = csv.GetField<string>("KodCiety"),
                            Pracownik = csv.GetField<string>("Pracownik"),
                            DokDostawy = csv.GetField<string>("DokDostawy"),
                            DataUtworzenia = csv.GetField<DateTime>("DataUtworzenia"),
                            DataDostawy = csv.GetField<DateTime>("DataDostawy"),
                            Komplet = csv.GetField<bool>("Komplet"),
                            Deklarowany = csv.GetField<bool>("Deklarowany"),
                            autocompleteEnabled = csv.GetField<bool>("autocompleteEnabled"),
                            wymuszonaIlosc = csv.GetField<bool>("wymuszonaIlosc"),
                            DataDopisu = csv.GetField<DateTime?>("DataDopisu"),
                            DopisanaIlosc = csv.GetField<int>("DopisanaIlosc"),
                            Uwagi = csv.GetField<string>("Uwagi"),
                            Technical = await _db.Technical.FindAsync(csv.GetField<string>("CietyWiazka")),
                            Dostawy = new List<VtToDostawa>()
                        };
                        await _db.VTMagazyn.AddAsync(record);
                        //await _db.VTMagazyn.AddAsync(csv.GetRecord<VTMagazyn>());
                        i++;

                        if (i >= chunkSize)
                        {
                            await _db.SaveChangesAsync();
                            i = 0;
                        }
                    }
                }
            }
            await _db.SaveChangesAsync();
            if (System.IO.File.Exists(backupDir + "/VtToDostawa.csv"))
            {
                using (var reader = new StreamReader(backupDir + "/VtToDostawa.csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    _db.RemoveRange(_db.VtToDostawa);
                    int i = 0;
                    await csv.ReadAsync();
                    csv.ReadHeader();
                    while (await csv.ReadAsync())
                    {
                        var vttd = new VtToDostawa { 
                            VTMagazynId = csv.GetField<Guid>("VTMagazynId"),
                            DostawaId = csv.GetField<string>("DostawaId")
                        };
                        var vt = _db.VTMagazyn.Include(c=>c.Dostawy).FirstOrDefault(c => c.VTMagazynId == vttd.VTMagazynId);
                        var dost = _db.Dostawa.Include(c=>c.Skany).FirstOrDefault(c => c.KodIloscData == vttd.DostawaId);
                        vttd.Dostawa = dost;
                        vttd.VTMagazyn = vt;
                        vt.Dostawy.Add(vttd);
                        dost.Skany.Add(vttd);
                        _db.Update(dost);
                        _db.Update(vt);
                        
                        await _db.VtToDostawa.AddAsync(vttd);
                        i++;

                        if (i >= chunkSize)
                        {
                            await _db.SaveChangesAsync();
                            i = 0;
                        }
                    }
                }
            }

            await _db.SaveChangesAsync();

            return RedirectToPage("/Manager/AdminManager/LoadDbFromFiles");
        }
    }
}