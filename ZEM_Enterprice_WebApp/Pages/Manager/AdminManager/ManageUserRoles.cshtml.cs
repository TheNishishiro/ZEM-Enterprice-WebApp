using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZEM_Enterprice_WebApp.Data;
using ZEM_Enterprice_WebApp.Utilities;

namespace ZEM_Enterprice_WebApp.Pages.Manager.AdminManager
{
    [Authorize(Roles = "Admin")]
    public class ManageUserRolesModel : PageModel
    {
        private readonly UserManager<MyUser> _userManager;
        private readonly SignInManager<MyUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageUserRolesModel(RoleManager<IdentityRole> roleManager, UserManager<MyUser> userManager, SignInManager<MyUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public List<InputModel> Input { get; set; }
        public class InputModel
        {
            [Required]
            [Display(Name = "Rola do przypisania/odebrania")]
            public string RoleName { get; set; }

            [Required]
            [Display(Name = "Dodaj rolę")]
            public bool Add { get; set; }
        }

        public async Task OnGet(Guid id)
        {
            Input = new List<InputModel>();

            var roleList = _roleManager.Roles.Select(r => new { r.Id, r.Name }).ToList();
            var user = await _userManager.FindByIdAsync(id.ToString());

            foreach (var role in roleList)
            {
                Input.Add(new InputModel { RoleName = role.Name, Add = await _userManager.IsInRoleAsync(user, role.Name) });
            }

            return;
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (ModelState.IsValid)
            {
                foreach (var inp in Input)
                {
                    if (inp.Add)
                    {
                        var user = await _userManager.FindByIdAsync(id);
                        await _userManager.AddToRoleAsync(user, inp.RoleName);
                    }
                    else
                    {
                        var user = await _userManager.FindByIdAsync(id);
                        await _userManager.RemoveFromRoleAsync(user, inp.RoleName);
                    }
                }
                return RedirectToPage("/Manager/AdminManager/ManageUsers");
            }
            return Page();
        }
    }
}