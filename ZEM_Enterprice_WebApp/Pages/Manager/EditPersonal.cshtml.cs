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

namespace ZEM_Enterprice_WebApp.Pages.Manager
{
    [Authorize()]
    public class EditPersonalModel : PageModel
    {
        private readonly UserManager<MyUser> _userManager;
        private readonly SignInManager<MyUser> _signInManager;

        public EditPersonalModel(UserManager<MyUser> userManager, SignInManager<MyUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusHeader { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Imię")]
            public string Imie { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Nazwisko")]
            public string Nazwisko { get; set; }
        }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            Input = new InputModel
            {
                Imie = user.Imie,
                Nazwisko = user.Nazwisko
            };
            return;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound("Użytkownik nie odnaleziony.");

            if (!ModelState.IsValid)
            {
                //await LoadAsync(user);
                return Page();
            }

            if (Input.Imie != user.Imie)
                user.Imie = Input.Imie;
            if (Input.Nazwisko != user.Nazwisko)
                user.Nazwisko = Input.Nazwisko;

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusHeader = "Dane zmienione";
            StatusMessage = "Dane pomyślnie zaktualizowane.";
            return RedirectToPage("/Manager/Profile");
        }
    }
}