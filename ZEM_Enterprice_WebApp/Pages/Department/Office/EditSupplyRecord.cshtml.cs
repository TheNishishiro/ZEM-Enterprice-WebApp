using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using ZEM_Enterprice_WebApp.Data;

namespace ZEM_Enterprice_WebApp.Pages.Department.Office
{
    [Authorize(Policy = "AdminOrOffice")]
    public class EditSupplyRecordModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public EditSupplyRecordModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public InputModel Input { get; set; }


        public void OnGet(string id)
        {
            var record = _db.Dostawa.Find(Guid.Parse(id));
            if (record == null)
                return;
            Input = new InputModel();
            Input.Kod = record.Kod;
            Input.Ilosc = record.Ilosc;
            Input.Data = record.Data;
            Input.Uwagi = record.Uwagi;
            Input.Id = record.DostawaId;
            Input.UploadDate = record.DataUtworzenia;
            Input.DeliveryDate = Input.Data;
        }

        public async Task<IActionResult> OnPostSaveChangesAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var record = await _db.Dostawa.FindAsync(Input.Id);

            record.Kod = Input.Kod;
            record.Ilosc = Input.Ilosc;
            record.Data = Input.Data;
            record.Uwagi = Input.Uwagi;

            _db.Dostawa.Update(record);
            await _db.SaveChangesAsync();

            return RedirectToPage("/Department/Office/ViewSupply");
        }

        public async Task<IActionResult> OnPostDeleteDeliveryAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _db.Dostawa.RemoveRange(_db.Dostawa.Where(c => c.Data.Date == Input.Data.Date && c.DataUtworzenia.Date == Input.DeliveryDate.Date));
            await _db.SaveChangesAsync();

            return RedirectToPage("/Department/Office/ViewSupply");
        }
    }

    public class InputModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Kod")]
        public string Kod { get; set; }
        [Required]
        [Display(Name = "Ilość")]
        public int Ilosc { get; set; }
        [Required]
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }
        [Display(Name = "Uwagi")]
        public string Uwagi { get; set; }

        [Display(Name = "Data utworzenia")]
        [DataType(DataType.Date)]
        public DateTime UploadDate { get; set; }

        [Required]
        [Display(Name = "Data dostawy")]
        [DataType(DataType.Date)]
        public DateTime DeliveryDate { get; set; }
    }

}