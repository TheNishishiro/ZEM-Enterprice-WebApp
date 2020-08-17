using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ZEM_Enterprice_WebApp.Data;

namespace ZEM_Enterprice_WebApp.Pages.Department.Technical
{
    [Authorize(Policy = "AdminOrTech")]
    public class EditTechRecordModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public EditTechRecordModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public InputModel Input { get; set; }


        public async Task OnGetAsync(string id)
        {
            var record = await _db.Technical.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.CietyWiazka == id);
            if (record == null)
                return;
            Input = new InputModel();
            Input.CietyWiazka = record.CietyWiazka;
            Input.BIN = record.BIN;
            Input.IlePrzewodow = record.IlePrzewodow;
            Input.IndeksScala = record.IndeksScala;
            Input.KanBan = record.KanBan;
            Input.KodWiazki = record.KodWiazki;
            Input.LiterRodziny = record.LiterRodziny;
            Input.PrzewodCiety = record.PrzewodCiety;
            Input.Rodzina = record.Rodzina;
            Input.Uwagi = record.Uwagi;
            Input.Wiazka = record.Wiazka;
            Input.isDeleted = record.Deleted;

            return;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var record = await _db.Technical.IgnoreQueryFilters().FirstOrDefaultAsync(c => c.CietyWiazka == Input.CietyWiazka);

            record.CietyWiazka = Input.Wiazka + "_" + Input.PrzewodCiety;
            record.BIN = Input.BIN;
            record.IlePrzewodow = Input.IlePrzewodow;
            record.IndeksScala = Input.IndeksScala;
            record.KanBan = Input.KanBan;
            record.KodWiazki = Input.KodWiazki;
            record.LiterRodziny = Input.LiterRodziny;
            record.PrzewodCiety = Input.PrzewodCiety;
            record.Rodzina = Input.Rodzina;
            record.Uwagi = Input.Uwagi;
            record.Wiazka = Input.Wiazka;
            record.Deleted = Input.isDeleted;

            _db.Technical.Update(record);
            await _db.SaveChangesAsync();

            return RedirectToPage("/Department/Technical/ViewTechResults");
        }
    }

    public class InputModel
    {
        [Required]
        public string CietyWiazka { get; set; }
        [Required]
        [Display(Name = "Rodzina")]
        public string Rodzina { get; set; }
        [Required]
        [Display(Name = "Wiązka")]
        public string Wiazka { get; set; }
        [Required]
        [Display(Name = "Litera rodziny")]
        public string LiterRodziny { get; set; }
        [Required]
        [Display(Name = "Kod wiązki")]
        public string KodWiazki { get; set; }
        [Required]
        [Display(Name = "Ilość przewodów")]
        public string IlePrzewodow { get; set; }
        [Required]
        [Display(Name = "Przewód cięty")]
        public string PrzewodCiety { get; set; }
        [Required]
        [Display(Name = "BIN")]
        public string BIN { get; set; }
        [Required]
        [Display(Name = "Indeks scala")]
        public string IndeksScala { get; set; }
        [Required]
        [Display(Name = "Kan-ban")]
        public bool KanBan { get; set; }
        [Display(Name = "Uwagi")]
        public string Uwagi { get; set; }
        [Required]
        [Display(Name = "Usunięty")]
        public bool isDeleted { get; set; }
    }

}