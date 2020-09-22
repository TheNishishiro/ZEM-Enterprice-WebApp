using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZEM_Enterprice_WebApp.Data;

namespace ZEM_Enterprice_WebApp.Utilities
{
    public enum DefaultRoles
    {
        Admin,
        Tech,
        Scanner,
        Office,
        Magazyn,
        Produkcja,
        Zarzadzanie_Skanami
    }

    public enum DefaultRequirements
    {
        AdminOrTech,
        AdminOrScanner,
        AdminOrOffice,
        AdminOrMagazyn,
        AdminOrProd,
        CanViewVT,
        CanViewSupply,
        CanViewVTScans,
        CanDoAnal
    }

    public class CreateRoles
    {

        // Initialize all basic roles for page viewing 
        public static void Default(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<MyUser>>();
            IdentityResult roleResult;

            string[] roleNames = Enum.GetNames(typeof(DefaultRoles));


            foreach (string roleName in roleNames)
            {
                var roleExists = RoleManager.RoleExistsAsync(roleName).Result;
                if(!roleExists)
                {
                    roleResult = RoleManager.CreateAsync(new IdentityRole(roleName)).Result;
                }
            }

            // Create an administrator account
            var superUser = UserManager.FindByNameAsync("Administrator").Result;
            if(superUser == null)
            {
                superUser = new MyUser { UserName = "Administrator", Imie = "Piotr", Nazwisko = "Kruszewski", EmailConfirmed = true };
                string userPWD = "P@55word";
                var result = UserManager.CreateAsync(superUser, userPWD).Result;
                if (result.Succeeded)
                {
                    UserManager.AddToRoleAsync(superUser, Enum.GetName(typeof(DefaultRoles), DefaultRoles.Admin));
                }
            }
        }
    }
}
