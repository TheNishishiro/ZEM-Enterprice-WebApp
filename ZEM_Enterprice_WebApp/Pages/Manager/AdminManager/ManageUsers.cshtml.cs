using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ZEM_Enterprice_WebApp.Data;

namespace ZEM_Enterprice_WebApp.Pages.Manager.AdminManager
{
    [Authorize(Roles = "Admin")]
    public class ManageUsersModel : PageModel
    {
        private readonly UserManager<MyUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public List<MyUser> Users;
        public List<IdentityRole> Roles;
        public ManageUsersModel(UserManager<MyUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task OnGet()
        {
            Users = await _userManager.Users.ToListAsync();
            Roles = await _roleManager.Roles.ToListAsync();
            return;
        }

        public async Task<IActionResult> OnPostActivateAsync(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            user.EmailConfirmed = !user.EmailConfirmed;
            await _userManager.UpdateAsync(user);
            await OnGet();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(string Id)
        {
            await _userManager.DeleteAsync(await _userManager.FindByIdAsync(Id));
            await OnGet();
            return Page();
        }
    }
}