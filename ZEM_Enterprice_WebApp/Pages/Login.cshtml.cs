using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ZEM_Enterprice_WebApp.Data;

namespace ZEM_Enterprice_WebApp.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<MyUser> _userManager;
        private readonly SignInManager<MyUser> _signInManager;

        public LoginModel(UserManager<MyUser> userManager, SignInManager<MyUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }


        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(Input.Username);
                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError(String.Empty, "Konto nie zostało jeszcze zatwierdzone.");
                    return Page();
                }

                var result = await _signInManager.PasswordSignInAsync(Input.Username, Input.Password, false, false);
                
                
                if(result.Succeeded)
                {
                    return RedirectToPage("/MainPage");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Nieprawidłowe dane");
                    return Page();
                }
            }

            return Page();
        }
    }
}