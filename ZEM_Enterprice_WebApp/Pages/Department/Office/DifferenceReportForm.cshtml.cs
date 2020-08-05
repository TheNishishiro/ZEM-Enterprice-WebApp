using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ZEM_Enterprice_WebApp.Pages.Department.Office
{
    [Authorize(Policy = "CanViewVTScans")]
    public class DifferenceReportFormModel : PageModel
    {
        [BindProperty]
        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Data do raportu")]
        public DateTime date { get; set; }

        public void OnGet()
        {
            date = DateTime.Now;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            return RedirectToPage($"/Department/Office/DifferenceReport", new { day = date.Day, month = date.Month, year = date.Year });
        }
    }
}