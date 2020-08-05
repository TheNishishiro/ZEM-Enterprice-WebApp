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

namespace ZEM_Enterprice_WebApp.Pages
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<MyUser> _signInManager;
        private readonly UserManager<MyUser> _userManager;

        [TempData]
        public string Message { get; set; }

        [BindProperty]
        public InputModel NewUser { get; set; }

        public class InputModel { 
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Imie { get; set; }

            [Required]
            public string Nazwisko { get; set; }

            [Required]
            public string UserName { get; set; }

            public bool isTechDep { get; set; }
            public bool isScanDep { get; set; }

            public bool isOfficeDep { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public RegisterModel(ApplicationDbContext db, SignInManager<MyUser> signInManager, UserManager<MyUser> userManager)
        { 
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(ModelState.IsValid)
            {
                var user = new MyUser { Email = NewUser.Email, Imie = NewUser.Imie, Nazwisko = NewUser.Nazwisko, UserName = NewUser.UserName, EmailConfirmed = false };
                var result = await _userManager.CreateAsync(user, NewUser.Password);
                if(result.Succeeded)
                {
                    Message = $"Stworzono użytkownika {user.Imie} {user.Nazwisko}";
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToPage("/Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}