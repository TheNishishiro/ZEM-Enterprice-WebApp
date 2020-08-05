using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using ZEM_Enterprice_WebApp.Data;
using ZEM_Enterprice_WebApp.Data.Tables;

namespace ZEM_Enterprice_WebApp.Pages.Department.Office
{
    [Authorize(Policy = "CanViewSupply")]
    public class UploadSupplyFileModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        [Required]
        public IFormFile dataFile { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data dostawy")]
        public DateTime date { get; set; }

        public UploadSupplyFileModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            using (StreamReader sr = new StreamReader(dataFile.OpenReadStream()))
            {
                while (sr.Peek() >= 0)
                {
                    string[] fields = sr.ReadLine().Split(',');
                    var tech = _db.Technical.FirstOrDefault(c => c.IndeksScala == fields[0]);
                    if (tech != null)
                    {
                        Dostawa dostawa = new Dostawa
                        {
                            KodIloscData = fields[0] + fields[1] + date.ToShortDateString(),
                            Kod = fields[0],
                            Ilosc = int.Parse(fields[1]),
                            Data = date,
                            Uwagi = "",
                            DataUtworzenia = DateTime.Now,
                            Technical = tech
                        };
                        await _db.Dostawa.AddAsync(dostawa);
                    }
                    else
                    {
                        PendingDostawa pDostawa = new PendingDostawa
                        {
                            KodIloscData = fields[0] + fields[1] + date.ToShortDateString(),
                            Kod = fields[0],
                            Ilosc = int.Parse(fields[1]),
                            Data = date,
                            Uwagi = ""
                        };
                        await _db.PendingDostawa.AddAsync(pDostawa);
                    }
                }
            }

            await _db.SaveChangesAsync();
            return RedirectToPage("/Department/Office/ConfirmMissing");
        }

    }
}