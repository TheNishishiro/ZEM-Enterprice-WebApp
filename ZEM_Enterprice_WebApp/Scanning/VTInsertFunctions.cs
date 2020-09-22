using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ZEM_Enterprice_WebApp.Data;
using ZEM_Enterprice_WebApp.Data.Tables;

namespace ZEM_Enterprice_WebApp.Scanning
{
    public class VTInsertFunctions
    {
        ApplicationDbContext _db;
        ScanCache _scanCache;

        public VTInsertFunctions(ApplicationDbContext db, ScanCache scanCache)
        {
            _db = db;
            _scanCache = scanCache;
        }

        /// <summary>
        /// Update relation between Dostawa and VTMagazyn when adding to already existing scan
        /// </summary>
        /// <param name="dostawaEntry"></param>
        /// <param name="vt">Scan to update</param>
        public void UpdateVT(Dostawa dostawaEntry, VTMagazyn vt)
        {
            VtToDostawa vttd = new VtToDostawa();
            vttd.DostawaId = dostawaEntry.DostawaId;
            vttd.VTMagazynId = vt.VTMagazynId;
            vttd.VTMagazyn = vt;
            vttd.Dostawa = dostawaEntry;
            if (vt.Dostawy.FirstOrDefault(c => c.VTMagazynId == vttd.VTMagazynId && c.DostawaId == vttd.DostawaId) == null)
                vt.Dostawy.Add(vttd);
        }

        /// <summary>
        /// Browse old scans in hope to find somewhere to add current scan to
        /// </summary>
        /// <param name="technical"></param>
        /// <param name="scanned"></param>
        /// <param name="dostawa"></param>
        /// <returns></returns>
        public bool SearchBack(Technical technical, ScannedCode scanned, Dostawa dostawa)
        {
            if (!scanned.isLookingBack ||_scanCache.LookedBack == true)
                return false;

            // Find all scans between dates
            var pastScans = _db.VTMagazyn.Include(c => c.Dostawy).Where(c =>
                c.Wiazka == scanned.Wiazka &&
                c.DataDostawy.Date < scanned.dataDostawy.Date &&
                c.autocompleteEnabled == true &&
                c.DataDostawy.Date >= scanned.dataDostawy.Date.AddDays(-7))
                .OrderBy(c => c.DataDostawy).ToList().GroupBy(c => c.DataDostawy).Select(g => g.ToList()).ToList();

            var pastDeliveries = _db.Dostawa.AsNoTracking().Include(c => c.Technical)
                .Where(c => c.Technical.Wiazka == scanned.Wiazka &&
                c.Data.Date < scanned.dataDostawy.Date &&
                c.Data.Date > scanned.dataDostawy.Date.AddDays(-7)).ToList();

            foreach (var scanPerDate in pastScans)
            {
                // Look if there are any duplicate scans (multiple sets)
                var duplicateScans = scanPerDate.Where(c => c.KodCiety == scanned.kodCiety).OrderBy(c => c.NumerKompletu).ToList();
                int mostFrequentCount = 0;

                if (duplicateScans.Count > 0)
                {
                    foreach (var scan in duplicateScans)
                    {
                        mostFrequentCount = GetPossibleDeclaredValue(new ScannedCode { Wiazka = scan.Wiazka, dataDostawyOld = scan.DataDostawy }, scanPerDate, pastDeliveries, scan.NumerKompletu);

                        // If this scan have already been added set flags and notify user
                        if (scan.DataDopisu != null &&
                            ((DateTime)scan.DataDopisu).Date == scanned.dataDostawy.Date
                            && !scanned.isForcedInsert)
                        {
                            scanned.addedBefore = true;
                            return false;
                        }

                        // Check if we can add current scan to previous one
                        if (scan.SztukiZeskanowane + scanned.sztukiSkanowane == mostFrequentCount
                           && scan.SztukiZeskanowane != mostFrequentCount)
                        {
                            scan.SztukiZeskanowane += scanned.sztukiSkanowane;
                            scan.DataDopisu = scanned.dataDostawy;
                            scan.DopisanaIlosc = scanned.sztukiSkanowane;
                            scan.DostawaDopis = scanned.DokDostawy;
                            scanned.dataDostawyOld = scan.DataDostawy;
                            if (dostawa != null)
                                UpdateVT(dostawa, scan);
                            _db.Update(scan);
                            _db.SaveChanges();

                            return true;
                        }
                    }

                    // Check if any set is missing current scan
                    int[] setIDs = scanPerDate.Where(c => c.Komplet == false).Select(c => c.NumerKompletu).Distinct().ToArray();
                    foreach (int setID in setIDs)
                    {
                        mostFrequentCount = GetPossibleDeclaredValue(new ScannedCode { Wiazka = scanned.Wiazka, dataDostawyOld = scanPerDate[0].DataDostawy }, scanPerDate, pastDeliveries, setID);
                        // Only add on the previous date if "declared" value is the same as scanned
                        if (scanned.sztukiSkanowane == mostFrequentCount && duplicateScans.Where(c => c.NumerKompletu == setID).Select(c => c.KodCiety).FirstOrDefault() != scanned.kodCiety)
                        {
                            scanned.DataDopisu = scanned.dataDostawy;
                            scanned.DopisanaIlosc = scanned.sztukiSkanowane;
                            scanned.dataDostawyOld = scanPerDate[0].DataDostawy;
                            scanned.DostawaDopis = scanned.DokDostawy;
                            AddToVT(technical, scanned, dostawa, true);
                            return true;
                        }
                    }
                }
                else
                {
                    mostFrequentCount = GetPossibleDeclaredValue(new ScannedCode { Wiazka = scanned.Wiazka, dataDostawyOld = scanPerDate[0].DataDostawy }, scanPerDate, pastDeliveries, 0);
                    // Only add on the previous date if "declared" value is the same as scanned
                    if (scanned.sztukiSkanowane == mostFrequentCount)
                    {
                        scanned.DataDopisu = scanned.dataDostawy;
                        scanned.DopisanaIlosc = scanned.sztukiSkanowane;
                        scanned.dataDostawyOld = scanPerDate[0].DataDostawy;
                        scanned.DostawaDopis = scanned.DokDostawy;
                        AddToVT(technical, scanned, dostawa);
                        return true;
                    }
                }
            }

            _scanCache.LookedBack = true;
            _db.Update(_scanCache);
            _db.SaveChanges();
            return false;
        }

        //todo try to optimize the setID == 0 route
        /// <summary>
        /// Function tries to guess what's the declared value for a choosen set 
        /// </summary>
        /// <param name="scanned"></param>
        /// <param name="setID">ID of a set to get declared ammount for</param>
        /// <returns></returns>
        public int GetPossibleDeclaredValue(ScannedCode scanned, List<VTMagazyn> VTScans, List<Dostawa> Deliveries, int setID = 0)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<VTMagazyn> pastScans = new List<VTMagazyn>();
            List<Dostawa> declaredScans = new List<Dostawa>();

            // For first set try to guess based off of delivery file
            if (setID == 0)
            {
                pastScans = VTScans.Where(c =>
                        c.Wiazka == scanned.Wiazka &&
                        c.DataDostawy.Date == scanned.dataDostawyOld.Date &&
                        c.autocompleteEnabled == true &&
                        c.NumerKompletu == setID).ToList();

                if (pastScans.Count > 0 && pastScans[0].wymuszonaIlosc)
                    return pastScans[0].SztukiDeklarowane;

                declaredScans = Deliveries.Where(c => c.Data.Date == scanned.dataDostawyOld.Date && c.Technical.KanBan == false).ToList();
            }
            // For any subsequent set guess based off of current scans
            else
            {
                pastScans = VTScans.Where(c =>
                       c.DataDostawy.Date == scanned.dataDostawyOld.Date &&
                       c.autocompleteEnabled == true &&
                       c.NumerKompletu == setID).ToList();
            }

            // If declared value have been forced by manager use it
            if (pastScans.Count > 0 && pastScans[0].wymuszonaIlosc)
                return pastScans[0].SztukiDeklarowane;

            sw.Stop();
            long s = sw.ElapsedMilliseconds;
            if (declaredScans.Count > 0)
                return declaredScans.OrderByDescending(c => c.Ilosc).GroupBy(c => c.Ilosc).OrderByDescending(c => c.Count()).Select(c => c.Key).First();
            else if (pastScans.Count > 0)
                return pastScans.OrderByDescending(c => c.SztukiZeskanowane).GroupBy(i => i.SztukiZeskanowane).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
            else return 0;
        }

        /// <summary>
        /// Adds scan to the database
        /// </summary>
        /// <param name="technical"></param>
        /// <param name="scanned"></param>
        /// <param name="dostawa"></param>
        /// <param name="newCmplt">Does it need to create new set</param>
        public void AddToVT(Technical technical, ScannedCode scanned, Dostawa dostawa, bool newCmplt = false)
        {
            if (dostawa != null && dostawa.Uwagi != "")
                scanned.complete = true;

            // Set proper set ID depending which set ID is missing
            if (!newCmplt)
                scanned.NumerKompletu = 0;
            else
            {
                int nextKpl = 0;
                var SetNumbers = GetCompleteID(scanned);
                foreach (var kplNumber in SetNumbers)
                {
                    if (nextKpl != kplNumber)
                    {
                        break;
                    }
                    nextKpl++;
                }

                scanned.NumerKompletu = nextKpl;
            }



            VtToDostawa vtToDostawa = new VtToDostawa();
            vtToDostawa.Dostawa = dostawa;

            VTMagazyn vt = new VTMagazyn
            {
                NumerKompletu = scanned.NumerKompletu,
                SztukiZeskanowane = scanned.sztukiSkanowane,
                SztukiDeklarowane = 0,
                Wiazka = technical.Wiazka,
                KodCiety = technical.PrzewodCiety,
                Pracownik = scanned.User,
                DokDostawy = scanned.DokDostawy,
                DataUtworzenia = scanned.dataUtworzenia,
                DataDostawy = scanned.dataDostawyOld == DateTime.MinValue ? scanned.dataDostawy : scanned.dataDostawyOld,
                Komplet = scanned.complete,
                Deklarowany = scanned.Declared,
                DataDopisu = scanned.DataDopisu,
                DostawaDopis = scanned.DostawaDopis,
                DopisanaIlosc = scanned.DopisanaIlosc,
                Uwagi = scanned.Uwagi,
                autocompleteEnabled = true,
                wymuszonaIlosc = false,
                Technical = technical
            };
            vtToDostawa.VTMagazyn = vt;

            // Create a relation between Dostawa and VTMagazyn scan
            vt.Dostawy = new List<VtToDostawa>();
            if (dostawa != null)
                vt.Dostawy.Add(vtToDostawa);

            _db.VTMagazyn.Add(vt);
        }

        /// <summary>
        /// Get set ids for current cable id
        /// </summary>
        /// <param name="scanned"></param>
        /// <returns></returns>
        public List<int> GetCompleteID(ScannedCode scanned)
        {
            try
            {
                return _db.VTMagazyn.AsNoTracking().Where(c => c.KodCiety == scanned.kodCiety && c.DataDostawy.Date == scanned.dataDostawyOld.Date).Select(c => c.NumerKompletu).ToList();
            }
            catch (Exception ex)
            {
                return new List<int>();
            }
        }

        public bool shouldPrintSpecial(ScannedCode sc)
        {
            var scan = _db.VTMagazyn.Include(c => c.Technical).Where(
                c => c.DataDostawy == sc.dataDostawyOld &&
                c.Wiazka == sc.Wiazka &&
                c.Technical.BIN == sc.BIN &&
                c.NumerKompletu == sc.NumerKompletu &&
                c.KodCiety != sc.kodCiety).FirstOrDefault();
            if (scan != null)
                return false;

            return true;
        }

        /// <summary>
        /// Automates the process of completing bundle sets and setting proper values for amount of cables scanned per bundle
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="numToComplete">Amount of cables scanned counting towards completion of a bundle</param>
        /// <param name="numScanned">Amount of cables scanned overall for this bundle</param>
        /// <param name="numScannedToComplete">Amount of cables going into that bundle</param>
        /// <returns></returns>
        public bool checkComplete(ScannedCode sc, out int numToComplete, out int numScanned, out int numScannedToComplete)
        {
            int completeID = sc.NumerKompletu;

            List<VTMagazyn> Scanned = _db.VTMagazyn.Where(
                c => c.Wiazka == sc.Wiazka &&
                c.NumerKompletu == completeID &&
                c.DataDostawy.Date == sc.dataDostawyOld.Date).ToList();

            int possibleDeclared = GetPossibleDeclaredValue(sc
                , Scanned,  
                _db.Dostawa.AsNoTracking().Include(c => c.Technical)
                        .Where(c => c.Technical.Wiazka == sc.Wiazka && c.Data.Date == sc.dataDostawyOld.Date).ToList(),
                completeID);
            numToComplete = _db.Technical.AsNoTracking().Where(c => c.Wiazka == sc.Wiazka && c.KanBan == false).Count();
            
            numScanned = Scanned.Count();
            numScannedToComplete = Scanned.Where(c => possibleDeclared <= c.SztukiZeskanowane &&
                c.autocompleteEnabled == true).Count();


            // Update values depending on scans
            foreach (var scan in Scanned)
            {
                if (scan.autocompleteEnabled == true)
                {
                    if (numToComplete == numScannedToComplete)
                        scan.Komplet = true;
                    if (!scan.wymuszonaIlosc)
                        scan.SztukiDeklarowane = possibleDeclared;

                    scan.ZeskanowanychNaKomplet = numScannedToComplete;
                    scan.NaKomplet = numToComplete;
                }
            }

            _db.UpdateRange(Scanned);
            _db.SaveChanges();

            // decide whether bundle have been completed
            if (numToComplete == numScannedToComplete)
                return true;
            return false;
        }

        /// <summary>
        /// Returns previous scan if it exists
        /// </summary>
        /// <param name="sc"></param>
        /// <returns></returns>
        public VTMagazyn ExistsInVT(ScannedCode sc)
        {
            return _db.VTMagazyn.FirstOrDefault(c =>
                c.KodCiety == sc.kodCiety &&
                c.DataDostawy.Date == sc.dataDostawyOld.Date);
        }
        /// <summary>
        /// Returns a list of incomplete records of code in VT for a date
        /// </summary>
        /// <param name="sc"></param>
        /// <returns></returns>
        public List<VTMagazyn> ExistInVTList(ScannedCode sc)
        {
            return _db.VTMagazyn.Where(c => c.KodCiety == sc.kodCiety
                && c.DataDostawy.Date == sc.dataDostawyOld.Date)
                .Include(c => c.Dostawy).OrderBy(c => c.NumerKompletu).ThenBy(c => c.SztukiZeskanowane).ToList();
        }

        /// <summary>
        /// Returns a scan which current scan would complete perfectly
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="setID"></param>
        /// <returns></returns>
        public VTMagazyn GetPerfectMatchVT(ScannedCode sc, int setID, List<VTMagazyn> listOfVT, List<VTMagazyn> scansForDay, List<Dostawa> listOfDeliveries)
        {
            int declared = GetPossibleDeclaredValue(sc, scansForDay, listOfDeliveries, setID);
            return listOfVT.FirstOrDefault(c => c.SztukiZeskanowane + sc.sztukiSkanowane == declared &&
                c.autocompleteEnabled == true && c.NumerKompletu == setID);
        }

        /// <summary>
        /// Returns a list of scans which current scan could contribute to completing 
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="setID"></param>
        /// <returns></returns>
        public List<VTMagazyn> GetBelowDeclaredMatches(ScannedCode sc, int setID, List<VTMagazyn> listOfVT, List<VTMagazyn> scansForDay, List<Dostawa> listOfDeliveries)
        {
            int declared = GetPossibleDeclaredValue(sc, listOfVT, listOfDeliveries, setID);
            return listOfVT.Where(c => c.SztukiZeskanowane + sc.sztukiSkanowane < declared &&
                c.autocompleteEnabled == true && c.NumerKompletu == setID).OrderByDescending(c => c.SztukiZeskanowane).ToList();
        }

        /// <summary>
        /// Tries to add current scan as a whole
        /// </summary>
        /// <param name="response"></param>
        /// <param name="techEntry"></param>
        /// <param name="sc"></param>
        /// <param name="dostawaEntry"></param>
        /// <returns></returns>
        public bool AddOrCreateNewSet(ScannedResponse response, Technical techEntry, ScannedCode sc, Dostawa dostawaEntry)
        {
            VTMagazyn VT = ExistsInVT(sc);
            if (VT == null && !sc.addedBefore)
            {
                // If no instance of this scan exists
                AddToVT(techEntry, sc, dostawaEntry);
            }
            else if (VT == null && sc.addedBefore)
            {
                if (sc.isForcedInsert)
                {
                    // If instance of this code have been added to previous scans
                    AddToVT(techEntry, sc, dostawaEntry);
                }
                else
                {
                    response.Header = HeaderTypes.error;
                    response.Flag = FlagType.codeExistsBack;
                    return false;
                }
            }
            else
            {
                if (sc.isForcedInsert)
                {
                    var deliveriesForToday = _db.Dostawa.AsNoTracking().Include(c => c.Technical)
                            .Where(c => c.Technical.Wiazka == sc.Wiazka && c.Data.Date == sc.dataDostawyOld.Date).ToList();
                    var VTList = new List<VTMagazyn>();
                    VTList.Add(VT);
                    // ?
                    // If instance of this scan have been added for todays delivery
                    if (VT.SztukiZeskanowane < GetPossibleDeclaredValue(sc, VTList, deliveriesForToday))
                        return AddQuantityIncorrect(deliveriesForToday, response, techEntry, sc, dostawaEntry);
                    else
                        AddToVT(techEntry, sc, dostawaEntry, true);
                }
                else
                {
                    response.Header = HeaderTypes.error;
                    response.Flag = FlagType.codeExists;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Functions searches previous scans and adds current scan there or if it can't tries to add it for today
        /// </summary>
        /// <param name="response"></param>
        /// <param name="techEntry"></param>
        /// <param name="sc"></param>
        /// <param name="dostawaEntry"></param>
        /// <returns></returns>
        public bool CheckBackOrAdd(ScannedResponse response, Technical techEntry, ScannedCode sc, Dostawa dostawaEntry)
        {
            if (!SearchBack(techEntry, sc, dostawaEntry))
            {
                return AddOrCreateNewSet(response, techEntry, sc, dostawaEntry);
            }
            return true;
        }

        /// <summary>
        /// Functions searches previous scans and adds current scan there or if it can't tries to add it for today if scan wasn't complete according to delivery
        /// </summary>
        /// <param name="response"></param>
        /// <param name="techEntry"></param>
        /// <param name="sc"></param>
        /// <param name="dostawaEntry"></param>
        /// <returns></returns>
        public bool CheckBackOrAddQuantityIncorrect(ScannedResponse response, Technical techEntry, ScannedCode sc, Dostawa dostawaEntry)
        {
            if (!SearchBack(techEntry, sc, dostawaEntry))
            {
                var deliveriesForToday = _db.Dostawa.AsNoTracking().Include(c => c.Technical)
                            .Where(c => c.Technical.Wiazka == sc.Wiazka && c.Data.Date == sc.dataDostawyOld.Date).ToList();
                return AddQuantityIncorrect(deliveriesForToday, response, techEntry, sc, dostawaEntry);
            }
            return true;
        }
        
        /// <summary>
        /// Adds record to current scan or if it can't adds it as a brand new one
        /// </summary>
        /// <param name="response"></param>
        /// <param name="techEntry"></param>
        /// <param name="sc"></param>
        /// <param name="dostawaEntry"></param>
        /// <returns></returns>
        public bool AddQuantityIncorrect(List<Dostawa> deliveriesForToday, ScannedResponse response, Technical techEntry, ScannedCode sc, Dostawa dostawaEntry)
        {
            if (!sc.isForcedQuantity)
            {
                response.Header = HeaderTypes.error;
                response.Flag = FlagType.quantityIncorrect;
                response.Args.Add(dostawaEntry.Ilosc.ToString());
                var sets = _db.VTMagazyn.Where(c => c.Wiazka == sc.Wiazka && c.DataDostawy == sc.dataDostawyOld).ToList();
                int declared = GetPossibleDeclaredValue(sc, sets, deliveriesForToday, sc.NumerKompletu);
                response.Args.Add(GetScannedForDay(sc, sets).ToString());
                response.Args.Add(declared.ToString());
                response.Args.Add($"{declared - sc.sztukiSkanowane}");
                return false;
            }
            else
            {
                var VTList = ExistInVTList(sc);
                var scansForDay = _db.VTMagazyn.Where(c => c.Wiazka == sc.Wiazka && c.DataDostawy.Date == sc.dataDostawyOld.Date).ToList();

                if (VTList.Count <= 0 && !sc.addedBefore)
                {
                    // if record doesn't exist add it
                    AddToVT(techEntry, sc, dostawaEntry);
                }
                else if (VTList.Count <= 0 && sc.addedBefore)
                {
                    if (sc.isForcedInsert)
                    {
                        // if record have been added previously add for today
                        AddToVT(techEntry, sc, dostawaEntry);
                    }
                    else
                    {
                        response.Header = HeaderTypes.error;
                        response.Flag = FlagType.codeExistsBack;
                        return false;
                    }
                }
                else
                {
                    List<VTMagazyn> forceOverLimit = new List<VTMagazyn>();
                    foreach (var vt in VTList)
                    {
                        // Check if we can add current scan to complete one today
                        var perfectVT = GetPerfectMatchVT(sc, vt.NumerKompletu, VTList, scansForDay, deliveriesForToday);
                        if (perfectVT != null)
                        {
                            perfectVT.SztukiZeskanowane += sc.sztukiSkanowane;
                            sc.NumerKompletu = perfectVT.NumerKompletu;
                            if (dostawaEntry != null)
                                UpdateVT(dostawaEntry, perfectVT);
                            _db.VTMagazyn.Update(perfectVT);
                            _db.SaveChanges();
                            return true;
                        }
                    }
                    foreach (var vt in VTList)
                    {
                        // check if there are any scans we can add current scan to
                        var belowMatches = GetBelowDeclaredMatches(sc, vt.NumerKompletu, VTList, scansForDay, deliveriesForToday);
                        if (belowMatches.Count() == 0)
                        {
                            if (vt.autocompleteEnabled)
                            {
                                int possibleDeclaredValue = GetPossibleDeclaredValue(sc, scansForDay, deliveriesForToday, vt.NumerKompletu);

                                // if adding would exceed declared value
                                if (vt.SztukiZeskanowane < possibleDeclaredValue && vt.SztukiZeskanowane + sc.sztukiSkanowane > possibleDeclaredValue)
                                {
                                    forceOverLimit.Add(vt);
                                }
                            }
                        }
                        else
                        {
                            belowMatches[0].SztukiZeskanowane += sc.sztukiSkanowane;
                            sc.NumerKompletu = belowMatches[0].NumerKompletu;
                            if (dostawaEntry != null)
                                UpdateVT(dostawaEntry, belowMatches[0]);
                            _db.VTMagazyn.Update(belowMatches[0]);
                            _db.SaveChanges();
                            return true;
                        }
                    }

                    if (forceOverLimit.Count() > 0)
                    {
                        if (!sc.isForcedOverLimit)
                        {
                            response.Header = HeaderTypes.error;
                            response.Flag = FlagType.quantityOverLimit;
                            response.Args.Add((forceOverLimit[0].SztukiZeskanowane + sc.sztukiSkanowane).ToString());
                            response.Args.Add(GetPossibleDeclaredValue(sc, scansForDay, deliveriesForToday, forceOverLimit[0].NumerKompletu).ToString());
                            return false;
                        }
                        else
                        {
                            forceOverLimit[0].SztukiZeskanowane += sc.sztukiSkanowane;
                            sc.NumerKompletu = forceOverLimit[0].NumerKompletu;
                            if (dostawaEntry != null)
                                UpdateVT(dostawaEntry, forceOverLimit[0]);
                            _db.VTMagazyn.Update(forceOverLimit[0]);
                            _db.SaveChanges();
                            return true;
                        }
                    }
                    else
                    {
                        if (sc.isForcedInsert)
                        {
                            AddToVT(techEntry, sc, dostawaEntry, true);
                            return true;
                        }
                        else
                        {
                            response.Header = HeaderTypes.error;
                            response.Flag = FlagType.codeExists;
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public int GetScannedForDay(ScannedCode sc, List<VTMagazyn> vtScans)
        {
            int forToday = vtScans.Where(c => c.KodCiety == sc.kodCiety && c.DataDostawy.Date == sc.dataDostawyOld.Date).Select(c => c.SztukiZeskanowane).Sum();
            int daysBack = _db.VTMagazyn.Where(c => c.KodCiety == sc.kodCiety && c.DataDopisu != null && ((DateTime)c.DataDopisu).Date == sc.dataDostawyOld.Date).Select(c => c.DopisanaIlosc).Sum();
            return forToday + daysBack;
        }
    }
}
