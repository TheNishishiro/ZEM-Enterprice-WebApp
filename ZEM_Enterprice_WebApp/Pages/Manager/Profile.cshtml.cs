using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ZEM_Enterprice_WebApp.Pages.Manager
{
    [Authorize()]
    public class ProfileModel : PageModel
    {
        [TempData]
        public string StatusHeader { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public void OnGet()
        {

        }
    }
}