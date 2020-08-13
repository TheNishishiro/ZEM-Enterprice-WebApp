using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ZEM_Enterprice_WebApp.Pages.Department.Scanner
{
    [Authorize(Policy = "CanViewVTScans")]
    public class ShowMissingModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}