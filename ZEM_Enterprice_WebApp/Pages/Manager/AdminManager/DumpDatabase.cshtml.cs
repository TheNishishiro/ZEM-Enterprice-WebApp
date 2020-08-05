using System;
using System.Collections.Generic;
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
    public class DumpDatabaseModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public DumpDatabaseModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task OnGetAsync()
        {
            if (!Directory.Exists("./Snapshots"))
                Directory.CreateDirectory("./Snapshots");

            string backupDir = $"./Snapshots/{DateTime.Now.Day}{DateTime.Now.Month}{DateTime.Now.Year}";

            if (!Directory.Exists(backupDir))
            {
                Directory.CreateDirectory(backupDir);

                int recordsInTable = _db.Technical.Count();
                int chunkSize = 20000;
                for(int i = 0; i < recordsInTable; i += chunkSize)
                {
                    var records = await _db.Technical.OrderBy(c => c.CietyWiazka).Skip(i).Take(chunkSize).ToListAsync();

                    using (var writer = new StreamWriter(backupDir + "/Technical.csv", true))
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

                recordsInTable = _db.VtToDostawa.Count();
                for (int i = 0; i < recordsInTable; i += chunkSize)
                {
                    var records = await _db.VtToDostawa.Skip(i).Take(chunkSize).ToListAsync();

                    using (var writer = new StreamWriter(backupDir + "/VtToDostawa.csv", true))
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

                recordsInTable = _db.VTMagazyn.Count();
                for (int i = 0; i < recordsInTable; i += chunkSize)
                {
                    var records = await _db.VTMagazyn.Skip(i).Take(chunkSize).ToListAsync();

                    using (var writer = new StreamWriter(backupDir + "/VTMagazyn.csv", true))
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

                recordsInTable = _db.Dostawa.Count();
                for (int i = 0; i < recordsInTable; i += chunkSize)
                {
                    var records = await _db.Dostawa.Skip(i).Take(chunkSize).ToListAsync();

                    using (var writer = new StreamWriter(backupDir + "/Dostawa.csv", true))
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

                recordsInTable = _db.MissingFromTech.Count();
                for (int i = 0; i < recordsInTable; i += chunkSize)
                {
                    var records = await _db.MissingFromTech.Skip(i).Take(chunkSize).ToListAsync();

                    using (var writer = new StreamWriter(backupDir + "/MissingFromTech.csv", true))
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

                recordsInTable = _db.PendingChangesTechnical.Count();
                for (int i = 0; i < recordsInTable; i += chunkSize)
                {
                    var records = await _db.PendingChangesTechnical.Skip(i).Take(chunkSize).ToListAsync();

                    using (var writer = new StreamWriter(backupDir + "/PendingChangesTechnical.csv", true))
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
            }

            return;
        }
    }
}