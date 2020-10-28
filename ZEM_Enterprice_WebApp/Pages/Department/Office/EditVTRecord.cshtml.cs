using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using ZEM_Enterprice_WebApp.Data;
using ZEM_Enterprice_WebApp.Data.Tables;
using ZEM_Enterprice_WebApp.Scanning;

namespace ZEM_Enterprice_WebApp.Pages.Department.Office
{
    [Authorize(Policy = "AdminOrOffice")]
    public class EditVTRecordModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public class InputModel
        {
            [Required]
            [Display(Name = "Jest kompletem")]
            public bool isCompleteSet { get; set; }
            [Required]
            [Display(Name = "Jest deklarowany")]
            public bool isDeclared { get; set; }
            [Required]
            [Display(Name = "Jest autokompletowany")]
            public bool isAutocompleteEnabled { get; set; }
            [Required]
            [Display(Name = "Posiada wymuszoną deklarację")]
            public bool isForcedDeclared { get; set; }
            [Required]
            [Display(Name = "Deklarowana ilość")]
            public int DeclaredValue { get; set; }
            [Required]
            [Display(Name = "Zeskanowana ilość")]
            public int ScannedValue { get; set; }
            [Required]
            [Display(Name = "Numer kompletu")]
            public int SetID { get; set; }
            [Required]
            [Display(Name = "Data dostawy")]
            [DataType(DataType.Date)]
            public DateTime DeliveryDate { get; set; }
            [Required]
            [Display(Name = "Wiązka")]
            public string Wiazka { get; set; }
            [Required]
            [Display(Name = "Kod cięty")]
            public string KodCiety { get; set; }
            [Required]
            [Display(Name = "Ilość kabli na komplet")]
            public int NaKomplet { get; set; }
            [Required]
            [Display(Name = "Ilość kabli zeskanowanych na komplet")]
            public int ZeskanowanychNaKomplet { get; set; }
            [Display(Name = "Uwagi")]
            public string Uwagi { get; set; }
            [Display(Name = "Dokument dostawy")]
            public string dokumentDostawy { get; set; }
            [Required]
            [Display(Name = "Data od")]
            [DataType(DataType.Date)]
            public DateTime DateFrom { get; set; }
            [Required]
            [Display(Name = "Data do")]
            [DataType(DataType.Date)]
            public DateTime DateTo { get; set; }
            [Display(Name = "Rozdziel skan na wartości")]
            public string SplitValue { get; set; }
            public Guid VTMagazynID { get; set; }

            [Display(Name = "Rozdziel skan na komplety")]
            public string SplitSetValue { get; set; }
            [Display(Name = "Połącz komplety")]
            public string MergeSetsString { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public EditVTRecordModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task OnGetAsync(string id)
        {
            var record = _db.VTMagazyn.Find(Guid.Parse(id));
            if (record == null)
                return;

            Input = new InputModel
            {
                isCompleteSet = record.Komplet,
                DateFrom = DateTime.Now.AddDays(-7),
                DateTo = DateTime.Now,
                DeclaredValue = record.SztukiDeklarowane,
                isAutocompleteEnabled = record.autocompleteEnabled,
                isDeclared = record.Deklarowany,
                isForcedDeclared = record.wymuszonaIlosc,
                KodCiety = record.KodCiety,
                ScannedValue = record.SztukiZeskanowane,
                SetID = record.NumerKompletu,
                SplitValue = "",
                Uwagi = record.Uwagi,
                VTMagazynID = record.VTMagazynId,
                Wiazka = record.Wiazka,
                DeliveryDate = record.DataDostawy,
                ZeskanowanychNaKomplet = record.ZeskanowanychNaKomplet,
                NaKomplet = record.NaKomplet,
                dokumentDostawy = record.DokDostawy
            };
            return;
        }

        public void UpdateWiazkaOnChange(VTMagazyn record, int originalValue)
        {
            if (record.SztukiZeskanowane == 0)
            {
                if (originalValue >= record.SztukiDeklarowane)
                {
                    var toUpdate = _db.VTMagazyn.Where(c =>
                        c.Wiazka == record.Wiazka &&
                        c.NumerKompletu == record.NumerKompletu &&
                        c.DataDostawy.Date == record.DataDostawy.Date).ToList();
                    foreach (var rec in toUpdate)
                    {
                        rec.ZeskanowanychNaKomplet--;
                        if (rec.autocompleteEnabled)
                            rec.Komplet = false;
                    }

                    _db.UpdateRange(toUpdate);
                }
                _db.VtToDostawa.RemoveRange(_db.VtToDostawa.Where(c => c.VTMagazynId == record.VTMagazynId));
                _db.VTMagazyn.Remove(record);
            }
            else if (record.SztukiZeskanowane < record.SztukiDeklarowane && originalValue >= record.SztukiDeklarowane)
            {
                if (originalValue >= record.SztukiDeklarowane)
                {
                    var toUpdate = _db.VTMagazyn.Where(c =>
                        c.Wiazka == record.Wiazka &&
                        c.NumerKompletu == record.NumerKompletu &&
                        c.DataDostawy.Date == record.DataDostawy.Date).ToList();
                    foreach (var rec in toUpdate)
                    {
                        rec.ZeskanowanychNaKomplet--;
                        if (rec.autocompleteEnabled)
                            rec.Komplet = false;
                    }

                    _db.UpdateRange(toUpdate);
                }
            }
        }

        public async Task<IActionResult> OnPostAcceptCableAsync(string id)
        {
            if (!ModelState.IsValid)
                return Page();

            var rec = _db.VTMagazyn.Find(Guid.Parse(id));
            if (rec != null)
            {
                rec.SztukiZeskanowane = Input.ScannedValue;
                rec.SztukiDeklarowane = Input.DeclaredValue;
                rec.Uwagi = Input.Uwagi;
                rec.Komplet = Input.isCompleteSet;
                rec.autocompleteEnabled = Input.isAutocompleteEnabled;
                rec.Deklarowany = Input.isDeclared;
                rec.wymuszonaIlosc = Input.isForcedDeclared;
                rec.NaKomplet = Input.NaKomplet;
                rec.ZeskanowanychNaKomplet = Input.ZeskanowanychNaKomplet;
                rec.DataDostawy = Input.DeliveryDate;
                _db.Update(rec);
                await _db.SaveChangesAsync();
            }
            await OnGetAsync(id);

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateDokDostawyDateAsync(string dokDostawy)
        {
            if (!ModelState.IsValid)
                return Page();

            var records = _db.VTMagazyn.Where(c => c.DokDostawy == dokDostawy);
            if (records.Count() > 0)
            {
                foreach(var record in records)
                {
                    record.DataDostawy = Input.DeliveryDate;
                }
                _db.UpdateRange(records);
            }
            await _db.SaveChangesAsync();
            await OnGetAsync(Input.VTMagazynID.ToString());

            return Page();
        }

        // Relation to delivery is not removed with dopis
        public async Task<IActionResult> OnPostDeleteDopisAsync(string id)
        {
            if (!ModelState.IsValid)
                return Page();

            var record = _db.VTMagazyn.Find(Guid.Parse(id));
            if(record != null)
            {
                int originalValue = record.SztukiZeskanowane;
                if (record.DataDopisu != null)
                {
                    record.SztukiZeskanowane -= record.DopisanaIlosc;

                    UpdateWiazkaOnChange(record, originalValue);

                    record.DataDopisu = null;
                    record.DopisanaIlosc = 0;
                }
            }
            await _db.SaveChangesAsync();
            await OnGetAsync(id);

            return Page();
        }

        public async Task<IActionResult> OnPostAcceptSetAsync(string wiazka)
        {
            if (!ModelState.IsValid)
                return Page();

            var records = await _db.VTMagazyn.Where(c => c.Wiazka == wiazka && c.DataDostawy.Date == Input.DeliveryDate.Date && c.NumerKompletu == Input.SetID).ToListAsync();
            if (records.Count > 0)
            {
                foreach(var rec in records)
                {
                    rec.SztukiDeklarowane = Input.DeclaredValue;
                    rec.Komplet = Input.isCompleteSet;
                    rec.autocompleteEnabled = Input.isAutocompleteEnabled;
                    rec.Deklarowany = Input.isDeclared;
                    rec.wymuszonaIlosc = Input.isForcedDeclared;
                    rec.NaKomplet = Input.NaKomplet;
                    rec.ZeskanowanychNaKomplet = Input.ZeskanowanychNaKomplet;
                    rec.DataDostawy = Input.DeliveryDate;
                    _db.Update(rec);
                }
                
                await _db.SaveChangesAsync();
            }

            await OnGetAsync(Input.VTMagazynID.ToString());

            return Page();
        }

        // optimized
        public async Task<IActionResult> OnPostMergeSetsCableAsync(string id)
        {
            if (!ModelState.IsValid)
                return Page();

            // 1, 0, 3
            string[] SetsToMerge = Input.MergeSetsString.Replace(" ", "").Split(',');
            var records = await _db.VTMagazyn.Where(c =>
                    c.Wiazka == Input.Wiazka &&
                    c.KodCiety == Input.KodCiety &&
                    c.DataDostawy.Date == Input.DeliveryDate.Date).ToListAsync();
            var record = records.FirstOrDefault(c => c.NumerKompletu == int.Parse(SetsToMerge[0]));

            int recordOriginalScanned = record.SztukiZeskanowane;

            for (int i = 1; i < SetsToMerge.Length; i++)
            {
                var rec = records.FirstOrDefault(c => c.NumerKompletu == int.Parse(SetsToMerge[i]));


                record.SztukiZeskanowane += rec.SztukiZeskanowane;
                int recScanned = rec.SztukiZeskanowane;
                _db.Update(record);
                rec.SztukiZeskanowane = 0;
                _db.Update(rec);
                await _db.SaveChangesAsync();
                UpdateWiazkaOnChange(rec, recScanned);
            }
            checkComplete(record.NumerKompletu, Input.Wiazka, Input.DeliveryDate);
            return Page();
        }

        public async Task<IActionResult> OnPostSplitSetCableAsync(string id)
        {
            if (!ModelState.IsValid)
                return Page();


            // 0:10, 1:5, 2:5
            string processedRequest = Input.SplitSetValue.Replace(" ", "");
            string[] SetValuePairs = processedRequest.Split(',');

            {
                int combinedValues = 0;
                foreach (var SetValuePair in SetValuePairs)
                {
                    combinedValues += int.Parse(SetValuePair.Split(':')[1]);
                }
                var record = _db.VTMagazyn.FirstOrDefault(c =>
                        c.Wiazka == Input.Wiazka &&
                        c.KodCiety == Input.KodCiety &&
                        c.DataDostawy.Date == Input.DeliveryDate.Date &&
                        c.NumerKompletu == Input.SetID);

                if(combinedValues != record.SztukiZeskanowane)
                {
                    ModelState.AddModelError("Niepoprawana ilość", " ilość w kompletach nie zgadza się z iloscią do rozbicia");
                    return Page();
                }
                    
            }

            var records = await _db.VTMagazyn.Where(c =>
                    c.Wiazka == Input.Wiazka &&
                    c.KodCiety == Input.KodCiety &&
                    c.DataDostawy.Date == Input.DeliveryDate.Date).ToListAsync();

            var deliveryRecords = await _db.Dostawa.IgnoreQueryFilters().AsNoTracking().Include(c =>c.Technical).Where(c =>
                    c.Technical.Wiazka == Input.Wiazka &&
                    c.Data.Date == Input.DeliveryDate.Date).ToListAsync();

            foreach (var SetValuePair in SetValuePairs)
            {
                int Set = int.Parse(SetValuePair.Split(':')[0]);
                int Value = int.Parse(SetValuePair.Split(':')[1]);

                var record = records.FirstOrDefault(c => c.NumerKompletu == Set);

                if (record != null)
                {
                    int original = record.SztukiZeskanowane;
                    record.SztukiZeskanowane = Value;
                    UpdateWiazkaOnChange(record, original);

                    //_db.Update(record);
                    await _db.SaveChangesAsync();
                }
                else if(Value != 0)
                {
                    int declaredValue = GetPossibleDeclaredValue(
                        new ScannedCode { Wiazka = Input.Wiazka , dataDostawyOld = Input.DeliveryDate },
                        records, deliveryRecords, Set);
                    if (declaredValue == 0)
                        declaredValue = Value;
                    
                    AddToVT(Set, Value, declaredValue, Input.dokumentDostawy, Input.DeliveryDate);
                    checkComplete(Set, Input.Wiazka, Input.DeliveryDate);
                }

                
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSplitCableAsync(string id)
        {
            if (!ModelState.IsValid)
                return Page();

            int[] values = Input.SplitValue.Split(',').Select(c => int.Parse(c.Trim())).ToArray();
            var record = await _db.VTMagazyn.FindAsync(Guid.Parse(id));
            int originalValue = record.SztukiZeskanowane;
            var pastScans = _db.VTMagazyn.Where(c => c.Wiazka == Input.Wiazka && c.DataDostawy.Date < Input.DateTo.Date && c.DataDostawy >= Input.DateFrom.Date && c.autocompleteEnabled == true)
                .OrderBy(c => c.DataDostawy).ToList().GroupBy(c => c.DataDostawy).Select(c => c.ToList()).ToList();
            var pastDeliveries = await _db.Dostawa.IgnoreQueryFilters().AsNoTracking().Include(c => c.Technical)
                .Where(c => c.Technical.Wiazka == Input.Wiazka &&
                c.Data.Date < Input.DateTo.Date &&
                c.Data.Date >= Input.DateFrom.Date).ToListAsync();

            foreach (int value in values)
            {
                if(SearchBack(pastScans, pastDeliveries, value, record))
                {
                    record.SztukiZeskanowane -= value;
                }
            }

            UpdateWiazkaOnChange(record, originalValue);

            await _db.SaveChangesAsync();
            await OnGetAsync(id);
            return Page();
        }

        public void checkComplete(int NumerKompletu, string Wiazka, DateTime dataDostawyOld)
        {
            int completeID = NumerKompletu;
            List<VTMagazyn> Scanned = _db.VTMagazyn.Where(
                c => c.Wiazka == Wiazka &&
                c.NumerKompletu == completeID &&
                c.DataDostawy.Date == dataDostawyOld.Date).ToList();

            int possibleDeclared = GetPossibleDeclaredValue(new ScannedCode { Wiazka = Wiazka, dataDostawyOld = dataDostawyOld}
                , Scanned,
                _db.Dostawa.IgnoreQueryFilters().AsNoTracking().Include(c => c.Technical)
                        .Where(c => c.Technical.Wiazka == Wiazka && c.Data.Date == dataDostawyOld.Date).ToList(),
                completeID);
            int numToComplete = _db.Technical.AsNoTracking().Where(c => c.Wiazka == Wiazka && c.KanBan == false).Count();
            int numScanned = Scanned.Count();
            int numScannedToComplete = Scanned.Where(c => possibleDeclared <= c.SztukiZeskanowane &&
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
        }

        private bool SearchBack(List<List<VTMagazyn>> pastScans, List<Dostawa> pastDeliveries, int value, VTMagazyn vtScan)
        {
            foreach(var scanPerDate in pastScans)
            {
                var duplicateScans = scanPerDate.Where(c => c.KodCiety == Input.KodCiety).OrderBy(c => c.NumerKompletu).ToList();
                int mostFrequentCount = 0;

                if (duplicateScans.Count > 0)
                {
                    foreach (var scan in duplicateScans)
                    {
                        mostFrequentCount = scan.SztukiDeklarowane;

                        if (scan.SztukiZeskanowane + value == mostFrequentCount
                           && scan.SztukiZeskanowane != mostFrequentCount)
                        {
                            scan.SztukiZeskanowane += value;
                            scan.DataDopisu = Input.DeliveryDate;
                            scan.DopisanaIlosc = value;
                            _db.Update(scan);
                            checkComplete(scan.NumerKompletu, Input.Wiazka, scan.DataDostawy);
                            return true;
                        }
                    }
                    int[] setIDs = scanPerDate.Where(c => c.Komplet == false).Select(c => c.NumerKompletu).Distinct().ToArray();
                    foreach (int setID in setIDs)
                    {
                        mostFrequentCount = GetPossibleDeclaredValue(
                            new ScannedCode { Wiazka = Input.Wiazka, dataDostawyOld = scanPerDate[0].DataDostawy },
                            scanPerDate, pastDeliveries, setID);
                        // Only add on the previous date if "declared" value is the same as scanned
                        if (value == mostFrequentCount && duplicateScans.Where(c => c.NumerKompletu == setID).Select(c => c.KodCiety).FirstOrDefault() != vtScan.KodCiety)
                        {
                            AddToVT(0, value, mostFrequentCount, vtScan.DokDostawy, scanPerDate[0].DataDostawy);
                            checkComplete(0, Input.Wiazka, scanPerDate[0].DataDostawy);
                            return true;
                        }
                    }
                }
                else
                {
                    mostFrequentCount = scanPerDate.Where(c => c.NumerKompletu == 0).First().SztukiDeklarowane;
                    // Only add on the previous date if "declared" value is the same as scanned
                    if (value == mostFrequentCount)
                    {
                        DateTime date = scanPerDate.Where(c => c.NumerKompletu == 0).FirstOrDefault().DataDostawy;
                        AddToVT(0, value, mostFrequentCount, vtScan.DokDostawy, date);
                        checkComplete(0, Input.Wiazka, date);
                        return true;
                    }
                }
            }

            return false;
        }

        public int GetPossibleDeclaredValue(ScannedCode scanned, List<VTMagazyn> VTScans, List<Dostawa> Deliveries, int setID = 0)
        {
            List<VTMagazyn> pastScans = new List<VTMagazyn>();
            List<Dostawa> declaredScans = new List<Dostawa>();

            // For first set try to guess based off of delivery file
            if (setID == 0)
            {
                pastScans = VTScans.Where(c =>
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

            if (declaredScans.Count > 0)
                return declaredScans.OrderByDescending(c => c.Ilosc).GroupBy(c => c.Ilosc).OrderByDescending(c => c.Count()).Select(c => c.Key).First();
            else if (pastScans.Count > 0)
                return pastScans.OrderByDescending(c => c.SztukiZeskanowane).GroupBy(i => i.SztukiZeskanowane).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
            else return 0;
        }

        private void AddToVT(int SetID, int value, int declared, string dokDostawy, DateTime dataDostawy)
        {
            VTMagazyn vt = new VTMagazyn
            {
                NumerKompletu = SetID,
                SztukiZeskanowane = value,
                SztukiDeklarowane = declared,
                Wiazka = Input.Wiazka,
                KodCiety = Input.KodCiety,
                Pracownik = User.Identity.Name,
                DokDostawy = dokDostawy,
                DataUtworzenia = DateTime.Now,
                DataDostawy = dataDostawy,
                Komplet = false,
                Deklarowany = Input.isDeclared,
                DataDopisu = Input.DeliveryDate,
                DopisanaIlosc = value,
                Uwagi = Input.Uwagi,
                autocompleteEnabled = Input.isAutocompleteEnabled,
                wymuszonaIlosc = Input.isForcedDeclared,
                Technical = _db.Technical.IgnoreQueryFilters().FirstOrDefault(c => c.PrzewodCiety == Input.KodCiety && c.Wiazka == Input.Wiazka),
            };
            vt.Dostawy = new List<VtToDostawa>();

            _db.VTMagazyn.Add(vt);
            _db.SaveChanges();
        }
    }
}