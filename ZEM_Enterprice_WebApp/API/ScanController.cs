using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZEM_Enterprice_WebApp.Data;
using ZEM_Enterprice_WebApp.Scanning;

namespace ZEM_Enterprice_WebApp.API
{
    [Route("api/missingScan")]
    [ApiController]
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
            VTInsertFunctions vTInsert = new VTInsertFunctions(_db);
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
            var SetIDs = vTInsert.GetCompleteID(new ScannedCode { kodCiety = cutcode, dataDostawyOld = deliveryDate });
            if (SetIDs.Count() == 0)
                SetIDs.Add(0);

            foreach (int setNumber in SetIDs)
            {
                var codesForWiazka = _db.Technical.Where(c => c.Wiazka == wiazka && c.KanBan == false).Select(c => c.PrzewodCiety).ToList();
                var scannedCodes = _db.VTMagazyn.Where(c => c.DataDostawy.Date == deliveryDate.Date && codesForWiazka.Contains(c.KodCiety) && c.NumerKompletu == setNumber).Select(c => c.KodCiety).ToList();
                if (scannedCodes.Count() == 0)
                {
                    return missingCodes;
                }


                missingCodes.Add($"Brakujące kody dla wiązki {wiazka} komplet nr. {setNumber} po {vTInsert.GetPossibleDeclaredValue(new ScannedCode { kodCiety = cutcode, Wiazka = wiazka, dataDostawyOld = deliveryDate }, setNumber)}");
                missingCodes.AddRange(codesForWiazka.Except(scannedCodes).ToList());
            }

            return missingCodes;
        }
    }
}
