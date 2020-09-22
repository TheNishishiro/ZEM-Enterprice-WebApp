using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZEM_Enterprice_WebApp.Data;
using ZEM_Enterprice_WebApp.Data.Tables;
using ZEM_Enterprice_WebApp.Pages.Department.Scanner;
using ZEM_Enterprice_WebApp.Scanning;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZEM_Enterprice_WebApp.API
{
    [Route("api/scannerInfo")]
    [ApiController]
    [AllowAnonymous]
    public class TechnicalController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public TechnicalController(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Returns all records for technical department table
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Technical>>> GetTech()
        {
            return await _db.Technical.ToListAsync();
        }

        /// <summary>
        /// Returns BIN for given cut-code
        /// </summary>
        /// <param name="przewod">Cable cut-code</param>
        /// <returns></returns>
        [HttpGet("{przewod}")]
        public async Task<ActionResult<PacketToSend>> GetTech(string przewod)
        {
            var wiazka = (await _db.Technical.FirstOrDefaultAsync(c => c.PrzewodCiety == przewod)).Wiazka;
            var result = await _db.Technical.Where(c => c.Wiazka == wiazka).Select(c => c.BIN).Distinct().ToListAsync();

            PacketToSend pts = new PacketToSend
            {
                StatusCode = 1,
            };

            if (result == null)
            { 
                pts.StatusCode = 0;
                return pts;
            }

            pts.BIN = result;

            return pts;
        }


        /// <summary>
        /// Manages scanning for web application
        /// </summary>
        /// <param name="kodWiazkiTextbox">Cable cut-code</param>
        /// <param name="forcedQuantity">Amount manualy inserted by user scanning</param>
        /// <param name="isLookingBack">Flag to control lookback for scans</param>
        /// <param name="dostDay">Day of delivery</param>
        /// <param name="dostMonth">Month of delivery</param>
        /// <param name="dostYear">Year of delivery</param>
        /// <param name="dokDostawy">Delivery document number</param>
        /// <param name="isForcedQuantity"></param>
        /// <param name="isForcedOverLimit"></param>
        /// <param name="isForcedBackAck"></param>
        /// <param name="isForcedBack"></param>
        /// <param name="isForcedInsert"></param>
        /// <param name="isForcedUndeclared"></param>
        /// <param name="isForcedOverDeclared"></param>
        /// <param name="User"></param>
        /// <returns></returns>
        [HttpGet("{sessionGUID},{kodWiazkiTextbox},{forcedQuantity},{isLookingBack},{dostYear}-{dostMonth}-{dostDay},{dokDostawy}," +
            "{isForcedQuantity},{isForcedOverLimit}," +
            "{isForcedBackAck},{isForcedBack},{isForcedInsert},{isForcedUndeclared},{isForcedOverDeclared}," +
            "{User}")]
        public async Task<ActionResult<ScannedResponse>> GetTech(
            string sessionGUID,
            string kodWiazkiTextbox,
            int forcedQuantity,
            bool isLookingBack,
            int dostDay, int dostMonth, int dostYear,
            string dokDostawy,
            bool isForcedQuantity,
            bool isForcedOverLimit,
            bool isForcedBackAck,
            bool isForcedBack,
            bool isForcedInsert,
            bool isForcedUndeclared,
            bool isForcedOverDeclared,
            string User
            )
        {
            // STO72301  210793
            var scan = _db.ScanCache.Find(Guid.Parse(sessionGUID));
            if(scan == null)
            {
                await _db.ScanCache.AddAsync(new ScanCache { ScanCacheId = Guid.Parse(sessionGUID) });
                await _db.SaveChangesAsync();
                scan = _db.ScanCache.Find(Guid.Parse(sessionGUID));
            }

            VTInsertFunctions VTFuncs = new VTInsertFunctions(_db, scan);
            ScannedResponse response = new ScannedResponse();
            ScannedCode sc = new ScannedCode();
            sc.kodCietyFull = kodWiazkiTextbox.ToUpper().Replace("PLC", "").Trim().Substring(0,8);
            if(forcedQuantity == 0)
                sc.sztukiSkanowane = int.Parse(kodWiazkiTextbox.ToUpper().Replace("PLC", "").Trim().Substring(8));
            else
                sc.sztukiSkanowane = forcedQuantity;
            sc.kodCiety = sc.kodCietyFull.TrimStart('0');

            sc.isLookingBack = isLookingBack;
            sc.dataDostawy = new DateTime(dostYear, dostMonth, dostDay);
            sc.DokDostawy = dokDostawy;
            sc.isForcedQuantity = isForcedQuantity;
            sc.isForcedOverLimit = isForcedOverLimit;
            sc.isForcedBackAck = isForcedBackAck;
            sc.isForcedBack = isForcedBack;
            sc.isForcedInsert = isForcedInsert;
            sc.isForcedUndeclared = isForcedUndeclared;
            sc.isForcedOverDeclared = isForcedOverDeclared;
            sc.User = User;
            sc.Declared = false;
            sc.complete = false;
            sc.isFullSet = false;
            sc.addedBefore = false;
            sc.dataDostawyOld = DateTime.MinValue;
            sc.dataDoskanowania = DateTime.Now;
            sc.dataUtworzenia = DateTime.Now;
            response.Args = new List<string>();
            response.sztukiSkanowane = sc.sztukiSkanowane;

            var techEntry = _db.Technical.IgnoreQueryFilters().FirstOrDefault(c => c.PrzewodCiety == sc.kodCiety);
            if (techEntry == null)
            {
                if (_db.MissingFromTech.Find(sc.kodCiety) == null)
                {
                    _db.MissingFromTech.Add(new MissingFromTech { DataDodania = sc.dataDoskanowania, Kod = sc.kodCiety, User = sc.User });
                    _db.SaveChanges();
                }
                response.Header = HeaderTypes.error;
                response.Flag = FlagType.notInTech;
                return response;
            }
            else if (techEntry.Deleted == true)
            {
                response.Header = HeaderTypes.error;
                response.Flag = FlagType.isDeleted;
                return response;

            }
            else if (techEntry.KanBan == true)
            {
                response.Header = HeaderTypes.error;
                response.Flag = FlagType.isKanban;
                return response;
            }

            sc.Wiazka = techEntry.Wiazka;
            sc.Rodzina = techEntry.Rodzina;
            sc.BIN = techEntry.BIN;

            response.PrzewodCiety = techEntry.PrzewodCiety;
            response.BIN = techEntry.BIN;
            response.KodWiazki = techEntry.KodWiazki;
            response.LiteraRodziny = techEntry.LiterRodziny;
            response.IlePrzewodow = techEntry.IlePrzewodow;

            var dostawaEntry = _db.Dostawa.FirstOrDefault(c => c.Data.Date == sc.dataDostawy.Date && c.Kod == "PLC" + sc.kodCietyFull);
            if (dostawaEntry != null)
            {
                sc.dataDostawy = dostawaEntry.Data;
                sc.dataDostawyOld = dostawaEntry.Data;
                //sc.isFullSet = VTFuncs.CheckIfFullSetOfSupply(sc);
                sc.sztukiDeklarowane = dostawaEntry.Ilosc;
                sc.Declared = true;

                // if codes to complete set are missing check back
                if (sc.sztukiSkanowane == sc.sztukiDeklarowane)
                {
                    var sets = _db.VTMagazyn.Where(c => c.Wiazka == sc.Wiazka && c.DataDostawy.Date == sc.dataDostawyOld.Date).ToList();
                    var deliveries = _db.Dostawa.Include(c => c.Technical).Where(c => c.Technical.Wiazka == sc.Wiazka && c.Data.Date == sc.dataDostawyOld.Date).ToList();
                    int declared = VTFuncs.GetPossibleDeclaredValue(sc, sets, deliveries, sc.NumerKompletu);

                    if (sc.sztukiSkanowane != declared && !sc.isForcedOverDeclared)
                    {
                        response.Header = HeaderTypes.error;
                        response.Flag = FlagType.quantityOverDeclated;
                        response.Args.Add(sc.sztukiDeklarowane.ToString());
                        response.Args.Add(VTFuncs.GetScannedForDay(sc, sets).ToString());
                        response.Args.Add(declared.ToString());
                        response.Args.Add($"{declared - sc.sztukiSkanowane}");

                        return response;
                    }
                    else
                    {
                        if (!VTFuncs.CheckBackOrAdd(response, techEntry, sc, dostawaEntry))
                            return response;
                    }
                }
                else if (sc.sztukiSkanowane != sc.sztukiDeklarowane)
                {
                    if (!sc.isForcedQuantity)
                    {
                        response.Header = HeaderTypes.error;
                        response.Flag = FlagType.quantityIncorrect;
                        response.Args.Add(sc.sztukiDeklarowane.ToString());
                        var sets = _db.VTMagazyn.Where(c => c.Wiazka == sc.Wiazka && c.DataDostawy.Date == sc.dataDostawyOld.Date).ToList();
                        var deliveries = _db.Dostawa.Include(c => c.Technical).Where(c => c.Technical.Wiazka == sc.Wiazka && c.Data.Date == sc.dataDostawyOld.Date).ToList();
                        int declared = VTFuncs.GetPossibleDeclaredValue(sc, sets, deliveries, sc.NumerKompletu);
                        response.Args.Add(VTFuncs.GetScannedForDay(sc, sets).ToString());

                        response.Args.Add(declared.ToString());
                        response.Args.Add($"{declared - sc.sztukiSkanowane}");

                        return response;
                    }
                    else
                    {
                        if (!VTFuncs.CheckBackOrAddQuantityIncorrect(response, techEntry, sc, dostawaEntry))
                            return response;
                    }
                }
            }
            else
            {
                sc.dataDostawy = sc.dataDostawy.Date;
                sc.dataDostawyOld = sc.dataDostawy.Date;

                if (!sc.isForcedUndeclared)
                {
                    response.Header = HeaderTypes.error;
                    response.Flag = FlagType.notInDeclared;
                    return response;
                }
                else
                {
                    if (!sc.isForcedQuantity)
                    {
                        response.Header = HeaderTypes.error;
                        response.Flag = FlagType.quantityIncorrect;
                        response.Args.Add("0");
                        var sets = _db.VTMagazyn.Where(c => c.Wiazka == sc.Wiazka && c.DataDostawy.Date == sc.dataDostawyOld.Date).ToList();
                        var deliveries = _db.Dostawa.Include(c => c.Technical).Where(c => c.Technical.Wiazka == sc.Wiazka && c.Data.Date == sc.dataDostawyOld.Date).ToList();
                        int declared = VTFuncs.GetPossibleDeclaredValue(sc, sets, deliveries, sc.NumerKompletu);
                        response.Args.Add(VTFuncs.GetScannedForDay(sc, sets).ToString());

                        response.Args.Add(declared.ToString());
                        response.Args.Add($"{declared - sc.sztukiSkanowane}");

                        return response;
                    }
                    else
                    {
                        if (!VTFuncs.CheckBackOrAddQuantityIncorrect(response, techEntry, sc, dostawaEntry))
                            return response;
                    }
                }
            }
            _db.SaveChanges();

            bool isComplete = VTFuncs.checkComplete(sc, out int numToComplete, out int numScanned, out int numScannedToComplete);

            response.DataDostawy = sc.dataDostawy;
            response.DataDostawyOld = sc.dataDostawyOld;
            response.numToComplete = numToComplete;
            response.numScanned = numScanned;
            response.numScannedToComplete = numScannedToComplete;
            response.isComplete = isComplete;
            response.sztukiDeklatowane = sc.sztukiDeklarowane;
            response.numerKompletu = sc.NumerKompletu;
            response.Wiazka = sc.Wiazka;
            response.Rodzina = sc.Rodzina;
            response.sztukiSkanowane = sc.sztukiSkanowane;

            if (numScanned == 1)
            {
                response.print = true;
                response.isSpecialColor = false;
            }
            if (VTFuncs.shouldPrintSpecial(sc))//_db.Technical.AsNoTracking().Where(c => c.Wiazka == sc.Wiazka).Select(c => c.BIN).Distinct().Count() > 1)
            {
                response.print = true;
                response.isSpecialColor = true;
            }

            return response;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ScanCache>> DeleteScanCache(string id)
        {
            var cache = await _db.ScanCache.FindAsync(Guid.Parse(id));
            if (cache == null)
                return NotFound();

            _db.ScanCache.Remove(cache);
            await _db.SaveChangesAsync();

            return cache;
        }

        public class PacketToSend
        {
            public int StatusCode { get; set; }
            public List<string> BIN { get; set; }
        }
    }
}
