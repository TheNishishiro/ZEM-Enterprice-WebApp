using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZEM_Enterprice_WebApp.Data;
using ZEM_Enterprice_WebApp.Data.Tables;
using ZEM_Enterprice_WebApp.Scanning;

namespace ZEM_Enterprice_WebApp.API
{
    [Route("api/missingScan")]
    [ApiController]
    [AllowAnonymous]
    public class ScanController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ScanController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("{przewod},{year}-{month}-{day}")]
        public async Task<ActionResult<List<string>>> GetMissing(string przewod, int year, int month, int day)
        {
            VTInsertFunctions vTInsert = new VTInsertFunctions(_db, null);
            string cutcode = przewod;

            List<string> missingCodes = new List<string>();
            DateTime deliveryDate = new DateTime(year, month, day);

            var techEntry = _db.Technical.FirstOrDefault(c => c.PrzewodCiety == cutcode);
            if (techEntry == null)
            {
                return missingCodes;
            }
            else if (techEntry.KanBan)
            {
                return missingCodes;
            }
            var wiazka = techEntry.Wiazka;
            var SetIDs = vTInsert.GetSetIDsForBundle(new ScannedCode { Wiazka = wiazka, dataDostawyOld = deliveryDate });
            if (SetIDs.Count() == 0)
                SetIDs.Add(0);

            var deliveries = await _db.Dostawa.IgnoreQueryFilters().AsNoTracking().Include(c => c.Technical).Where(c => c.Technical.Wiazka == wiazka &&
            c.Data.Date == deliveryDate.Date).ToListAsync();
            var scans = await _db.VTMagazyn.AsNoTracking().Where(c => c.Wiazka == wiazka && c.DataDostawy.Date == deliveryDate.Date).ToListAsync();

            foreach (int setNumber in SetIDs)
            {
                var codesForWiazka = _db.Technical.Where(c => c.Wiazka == wiazka && c.KanBan == false).Select(c => c.PrzewodCiety).ToList();
                var scannedCodes = scans.Where(c => c.NumerKompletu == setNumber).Select(c => c.KodCiety).ToList();
                if (scannedCodes.Count() == 0)
                {
                    return missingCodes;
                }


                missingCodes.Add($"Brakujące kody dla wiązki {wiazka} komplet nr. {setNumber} po {vTInsert.GetPossibleDeclaredValue(new ScannedCode { kodCiety = cutcode, Wiazka = wiazka, dataDostawyOld = deliveryDate }, scans, deliveries, setNumber)}");
                missingCodes.AddRange(codesForWiazka.Except(scannedCodes).ToList());
            }

            return missingCodes;
        }

        
    }
}
