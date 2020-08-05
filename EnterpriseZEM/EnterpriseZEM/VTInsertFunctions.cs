using EnterpriseZEM.db.tables;
using EnterpriseZEM_Common;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseZEM
{
    class VTInsertFunctions
    {
        ZEMDBContext _db;
        Serilog.Core.Logger _log;

        public VTInsertFunctions(ZEMDBContext db, Serilog.Core.Logger log)
        {
            _db = db;
            _log = log;
        }

        public void UpdateVT(Dostawa dostawaEntry, VTMagazyn vt)
        {
            VtToDostawa vttd = new VtToDostawa();
            vttd.DostawaId = dostawaEntry.KodIloscData;
            vttd.VTMagazynId = vt.VTMagazynId;
            vttd.VTMagazyn = vt;
            vttd.Dostawa = dostawaEntry;
            if (vt.Dostawy.Where(c => c.VTMagazynId == vttd.VTMagazynId && c.DostawaId == vttd.DostawaId) == null)
                vt.Dostawy.Add(vttd);
        }

        //todo Handle adding to different sets similar to how on current day is handling it (should work now)
        public bool SearchBack(Technical technical, ScannedCode scanned, Dostawa dostawa)
        {
            if (!scanned.isLookingBack)
                return false;

            var pastScans = _db.VTMagazyn.Where(c =>
                c.Wiazka == scanned.Wiazka &&
                c.DataDostawy.Date < scanned.dataDostawy.Date &&
                /*c.Komplet == false &&*/ c.autocompleteEnabled == true &&
                c.DataDostawy.Date > scanned.dataDostawy.Date.AddDays(-7))
                .OrderBy(c => c.DataDostawy).ToList().GroupBy(c => c.DataDostawy).Select(g => g.ToList()).ToList();

            foreach (var scanPerDate in pastScans)
            {
                var duplicateScans = scanPerDate.Where(c => c.KodCiety == scanned.kodCiety).OrderBy(c => c.NumerKompletu).ToList();
                int mostFrequentCount = 0;//scanPerDate.GroupBy(i => i.SztukiZeskanowane).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();

                if (duplicateScans.Count > 0)
                {
                    foreach (var scan in duplicateScans)
                    {
                        mostFrequentCount = GetPossibleDeclaredValue(new ScannedCode { Wiazka = scan.Wiazka, dataDostawyOld = scan.DataDostawy }, scan.NumerKompletu);

                        if (scan.DataDopisu != null &&
                            ((DateTime)scan.DataDopisu).Date == scanned.dataDostawy.Date
                            && !scanned.isForcedInsert)
                        {
                            scanned.addedBefore = true;
                            return false;
                        }

                        if (scan.SztukiZeskanowane + scanned.sztukiSkanowane == mostFrequentCount
                           && scan.SztukiZeskanowane != mostFrequentCount)
                        {
                            scan.SztukiZeskanowane += scanned.sztukiSkanowane;
                            scan.DataDopisu = scan.DataDostawy;
                            scan.DopisanaIlosc = scanned.sztukiSkanowane;
                            scanned.dataDostawyOld = scan.DataDostawy;
                            if(dostawa != null)
                                UpdateVT(dostawa, scan);
                            _db.Update(scan);
                            _db.SaveChanges();

                            return true;
                        }
                    }
                    int[] setIDs = scanPerDate.Where(c => c.Komplet == false).Select(c => c.NumerKompletu).Distinct().ToArray();
                    foreach (int setID in setIDs)
                    {
                        mostFrequentCount = GetPossibleDeclaredValue(new ScannedCode { Wiazka = scanned.Wiazka, dataDostawyOld = scanPerDate[0].DataDostawy }, setID);
                        // Only add on the previous date if "declared" value is the same as scanned
                        if (scanned.sztukiSkanowane == mostFrequentCount && duplicateScans.Where(c => c.NumerKompletu == setID).Select(c=>c.KodCiety).FirstOrDefault() != scanned.kodCiety)
                        {
                            scanned.DataDopisu = scanned.dataDostawy;
                            scanned.DopisanaIlosc = scanned.sztukiSkanowane;
                            scanned.dataDostawyOld = scanPerDate[0].DataDostawy;
                            AddToVT(technical, scanned, dostawa, true);
                            return true;
                        }
                    }
                }
                else 
                {
                    mostFrequentCount = GetPossibleDeclaredValue(new ScannedCode { Wiazka = scanned.Wiazka, dataDostawyOld = scanPerDate[0].DataDostawy }, 0);
                    // Only add on the previous date if "declared" value is the same as scanned
                    if (scanned.sztukiSkanowane == mostFrequentCount)
                    {
                        scanned.DataDopisu = scanned.dataDostawy;
                        scanned.DopisanaIlosc = scanned.sztukiSkanowane;
                        scanned.dataDostawyOld = scanPerDate[0].DataDostawy;
                        AddToVT(technical, scanned, dostawa);
                        return true;
                    }
                }
            }

            return false;
        }

        //todo try to optimize the setID == 0 route
        public int GetPossibleDeclaredValue(ScannedCode scanned, int setID = 0)
        {
            List<VTMagazyn> pastScans = new List<VTMagazyn>();
            List<Dostawa> declaredScans = new List<Dostawa>();

            if (setID == 0)
            {
                List<string> techCodes = _db.Technical.AsNoTracking().Where(c => c.Wiazka == scanned.Wiazka && c.KanBan == false).Select(c => c.IndeksScala).ToList();
                declaredScans = _db.Dostawa.AsNoTracking().Where(c => c.Data.Date == scanned.dataDostawyOld.Date && techCodes.Contains(c.Kod)).ToList();
                pastScans = _db.VTMagazyn.AsNoTracking().Where(c =>
                       c.Wiazka == scanned.Wiazka &&
                       c.DataDostawy.Date == scanned.dataDostawyOld.Date &&
                       c.autocompleteEnabled == true &&
                       c.NumerKompletu == setID).ToList();
            }
            else
            {
                pastScans = _db.VTMagazyn.AsNoTracking().Where(c =>
                       c.Wiazka == scanned.Wiazka &&
                       c.DataDostawy.Date == scanned.dataDostawyOld.Date &&
                       c.autocompleteEnabled == true &&
                       c.NumerKompletu == setID).ToList();
            }

            if (pastScans.Count > 0 && pastScans[0].wymuszonaIlosc)
                return pastScans[0].SztukiDeklarowane;

            if (declaredScans.Count > 0)
                return declaredScans.OrderByDescending(c => c.Ilosc).GroupBy(c => c.Ilosc).OrderByDescending(c => c.Count()).Select(c => c.Key).First();
            else if (pastScans.Count > 0)
                return pastScans.OrderByDescending(c => c.SztukiZeskanowane).GroupBy(i => i.SztukiZeskanowane).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
            else return 0;
        }
        //todo When creating new set check if there are any missing set id's before assigning new id, in case previous id have been deleted
        public void AddToVT(Technical technical, ScannedCode scanned, Dostawa dostawa, bool newCmplt = false)
        {
            if (dostawa != null && dostawa.Uwagi != "")
                scanned.complete = true;

            if (!newCmplt)
                scanned.NumerKompletu = 0;
            else
            {
                int nextKpl = 0;
                var SetNumbers = GetCompleteID(scanned);
                foreach(var kplNumber in SetNumbers)
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
                DopisanaIlosc = scanned.DopisanaIlosc,
                Uwagi = scanned.Uwagi,
                autocompleteEnabled = true,
                wymuszonaIlosc = false,
                Technical = technical
            };
            vtToDostawa.VTMagazyn = vt;

            vt.Dostawy = new List<VtToDostawa>();
            if(dostawa != null)
                vt.Dostawy.Add(vtToDostawa);

            _db.VTMagazyn.Add(vt);
        }
        public List<int> GetCompleteID(ScannedCode scanned)
        {
            try
            {
                return _db.VTMagazyn.AsNoTracking().Where(c => c.KodCiety == scanned.kodCiety && c.DataDostawy.Date == scanned.dataDostawyOld.Date).Select(c => c.NumerKompletu).ToList();
            }
            catch(Exception ex)
            {
                return new List<int>();
            }
        }
        public bool checkComplete(ScannedCode sc, out int numToComplete, out int numScanned, out int numScannedToComplete)
        {
            int completeID = sc.NumerKompletu;
            int possibleDeclared = GetPossibleDeclaredValue(sc, completeID);
            numToComplete = _db.Technical.AsNoTracking().Where(c => c.Wiazka == sc.Wiazka && c.KanBan == false).Count();
            List<VTMagazyn> Scanned = _db.VTMagazyn.Where(c => c.Wiazka == sc.Wiazka && c.DataDostawy.Date == sc.dataDostawyOld.Date && /*c.Komplet == false && c.autocompleteEnabled == true &&*/ c.NumerKompletu == completeID).ToList();
            numScanned = Scanned.Count();
            numScannedToComplete = _db.VTMagazyn.AsNoTracking().Where(c =>
                c.Wiazka == sc.Wiazka &&
                c.DataDostawy.Date == sc.dataDostawyOld.Date &&
                possibleDeclared <= c.SztukiZeskanowane &&
                /*c.Komplet == false &&*/ c.autocompleteEnabled == true &&
                c.NumerKompletu == completeID).Count();


            
            foreach (var scan in Scanned)
            {
                if (numToComplete == numScannedToComplete)
                    scan.Komplet = true;
                if (!scan.wymuszonaIlosc)
                    scan.SztukiDeklarowane = possibleDeclared;

                scan.ZeskanowanychNaKomplet = numScannedToComplete;
                scan.NaKomplet = numToComplete;
            }

            _db.UpdateRange(Scanned);
            _db.SaveChanges();

            if (numToComplete == numScannedToComplete)
                return true;
            return false;
        }

        public VTMagazyn ExistsInVT(ScannedCode sc)
        {
            return _db.VTMagazyn.FirstOrDefault(c =>
                c.KodCiety == sc.kodCiety &&
                c.DataDostawy.Date == sc.dataDostawyOld.Date);
        }
        // Returns a list of incomplete records of code in VT for a date
        public List<VTMagazyn> ExistInVTList(ScannedCode sc)
        {
            return _db.VTMagazyn.Where(c => c.KodCiety == sc.kodCiety 
                && c.DataDostawy.Date == sc.dataDostawyOld.Date)
                .Include(c => c.Dostawy).OrderBy(c => c.NumerKompletu).ThenBy(c => c.SztukiZeskanowane).ToList();
        }
        public VTMagazyn GetPerfectMatchVT(ScannedCode sc, int setID)
        {
            int declared = GetPossibleDeclaredValue(sc, setID);
            return _db.VTMagazyn.Include(c=>c.Dostawy).FirstOrDefault(c =>
                c.KodCiety == sc.kodCiety &&
                c.DataDostawy.Date == sc.dataDostawyOld.Date &&
                c.SztukiZeskanowane + sc.sztukiSkanowane == declared &&
                c.autocompleteEnabled == true && c.NumerKompletu == setID);
        }
        public List<VTMagazyn> GetBelowDeclaredMatches(ScannedCode sc, int setID)
        {
            int declared = GetPossibleDeclaredValue(sc, setID);
            return _db.VTMagazyn.Where(c =>
                c.KodCiety == sc.kodCiety &&
                c.DataDostawy.Date == sc.dataDostawyOld.Date &&
                c.SztukiZeskanowane + sc.sztukiSkanowane < declared &&
                c.autocompleteEnabled == true && c.NumerKompletu == setID).Include(c => c.Dostawy).ToList().OrderByDescending(c => c.SztukiZeskanowane).ToList();
        }
        public bool CheckIfFullSetOfSupply(ScannedCode sc)
        {
            List<string> techCodes = _db.Technical.AsNoTracking().Where(c => c.Wiazka == sc.Wiazka && c.KanBan == false).Select(c => c.IndeksScala).ToList();
            List<Dostawa> dostawaEntries = _db.Dostawa.AsNoTracking().Where(c => c.Data.Date == sc.dataDostawy.Date && techCodes.Contains(c.Kod)).ToList();
            if (techCodes.Count() == dostawaEntries.Count() && dostawaEntries.Select(c => c.Ilosc).Distinct().Count() == 1)
                return true;
            return false;
        }

        //todo Analize if this function works well with multiple sets (it should)
        public bool AddOrCreateNewSet(CustomPacket response, Technical techEntry, ScannedCode sc, Dostawa dostawaEntry)
        {
            VTMagazyn VT = ExistsInVT(sc);
            if (VT == null && !sc.addedBefore)
            {
                AddToVT(techEntry, sc, dostawaEntry);
            }
            else if (VT == null && sc.addedBefore)
            {
                if (sc.isForcedInsert)
                {
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
                    if (VT.SztukiZeskanowane < GetPossibleDeclaredValue(sc))
                        return AddQuantityIncorrect(response, techEntry, sc, dostawaEntry);
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
        public bool CheckBackOrAdd(CustomPacket response, Technical techEntry, ScannedCode sc, Dostawa dostawaEntry)
        {
            if (!SearchBack(techEntry, sc, dostawaEntry))
            {
                return AddOrCreateNewSet(response, techEntry, sc, dostawaEntry);
            }
            return true;
        }
        public bool CheckBackOrAddQuantityIncorrect(CustomPacket response, Technical techEntry, ScannedCode sc, Dostawa dostawaEntry)
        {
            if (!SearchBack(techEntry, sc, dostawaEntry))
            {
                return AddQuantityIncorrect(response, techEntry, sc, dostawaEntry);
            }
            return true;
        }
        //todo check next record if quantityOverLimit have been triggered (maybe fixed)
        public bool AddQuantityIncorrect(CustomPacket response, Technical techEntry, ScannedCode sc, Dostawa dostawaEntry)
        {
            if (!sc.isForcedQuantity)
            {
                response.Header = HeaderTypes.error;
                response.Flag = FlagType.quantityIncorrect;
                response.Args.Add(dostawaEntry.Ilosc.ToString());
                var vt = ExistsInVT(sc);
                if(vt != null)
                    response.Args.Add(vt.SztukiZeskanowane.ToString());
                else
                    response.Args.Add("0");
                return false;
            }
            else
            {
                var VTList = ExistInVTList(sc);
                if (VTList.Count <= 0 && !sc.addedBefore)
                {
                    AddToVT(techEntry, sc, dostawaEntry);
                }
                else if(VTList.Count <= 0 && sc.addedBefore)
                {
                    if (sc.isForcedInsert)
                    {
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
                        //if(sc.sztukiSkanowane == vt.SztukiZeskanowane && !sc.isForcedInsert)
                        //{
                        //    response.Header = HeaderTypes.error;
                        //    response.Flag = FlagType.codeExists;
                        //    return false;
                        //}

                        var perfectVT = GetPerfectMatchVT(sc, vt.NumerKompletu);
                        if (perfectVT != null)
                        {
                            perfectVT.SztukiZeskanowane += sc.sztukiSkanowane;
                            sc.NumerKompletu = perfectVT.NumerKompletu;
                            if(dostawaEntry != null)
                                UpdateVT(dostawaEntry, perfectVT);
                            _db.VTMagazyn.Update(perfectVT);
                            _db.SaveChanges();
                            return true;
                        }
                    }
                    foreach (var vt in VTList)
                    {
                        var belowMatches = GetBelowDeclaredMatches(sc, vt.NumerKompletu);
                        if (belowMatches.Count() == 0 )
                        {
                            if (vt.autocompleteEnabled)
                            {
                                int possibleDeclaredValue = GetPossibleDeclaredValue(sc, vt.NumerKompletu);

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
                            if(dostawaEntry != null)
                                UpdateVT(dostawaEntry, belowMatches[0]);
                            _db.VTMagazyn.Update(belowMatches[0]);
                            _db.SaveChanges();
                            return true;
                        }
                    }

                    if(forceOverLimit.Count() > 0)
                    {
                        if (!sc.isForcedOverLimit)
                        {
                            response.Header = HeaderTypes.error;
                            response.Flag = FlagType.quantityOverLimit;
                            response.Args.Add((forceOverLimit[0].SztukiZeskanowane + sc.sztukiSkanowane).ToString());
                            response.Args.Add(GetPossibleDeclaredValue(sc, forceOverLimit[0].NumerKompletu).ToString());
                            return false;
                        }
                        else
                        {
                            forceOverLimit[0].SztukiZeskanowane += sc.sztukiSkanowane;
                            sc.NumerKompletu = forceOverLimit[0].NumerKompletu;
                            if(dostawaEntry != null)
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
    }
}
