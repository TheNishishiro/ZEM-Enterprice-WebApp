using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZEM_Enterprice_WebApp.Data;
using ZEM_Enterprice_WebApp.Utilities;

namespace ZEM_Enterprice_WebApp.Pages.Manager.AdminManager
{
    [Authorize(Roles = "Admin")]
    public class ResetUserPasswordModel : PageModel
    {
        public class InputModel
        {
            [Display(Name = "Imie")]
            public string Imie { get; set; }
            [Display(Name = "Nazwisko")]
            public string Nazwisko { get; set; }
            public string Id { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        private readonly UserManager<MyUser> _userManager;
        private readonly SignInManager<MyUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ResetUserPasswordModel(RoleManager<IdentityRole> roleManager, UserManager<MyUser> userManager, SignInManager<MyUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public void OnGet(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            Input = new InputModel();
            Input.Imie = user.Imie;
            Input.Nazwisko = user.Nazwisko;
            Input.Id = id;
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {

            var user = await _userManager.FindByIdAsync(Input.Id);

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, code, user.UserName);
            if(result.Succeeded)
                return RedirectToPage("/Manager/AdminManager/ManageUsers");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}