using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ZEM_Enterprice_WebApp.Pages.Department.Office
{
    [Authorize(Policy = "CanDoAnal")]
    public class AnalizeFormModel : PageModel
    {
        [BindProperty]
        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Dzień analizy")]
        public DateTime date { get; set; }

        private readonly ILogger _logger;

        public AnalizeFormModel(ILogger<AnalizeFormModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            date = DateTime.Now;
            return;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            return RedirectToPage($"/Department/Office/AnalizeReport", new { 
                day = date.Day, month = date.Month, year = date.Year
            });
        }
    }
}