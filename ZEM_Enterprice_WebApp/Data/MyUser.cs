using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZEM_Enterprice_WebApp.Data
{
    public class MyUser : IdentityUser
    {
        [PersonalData]
        public string Imie { get; set; }
        [PersonalData]
        public string Nazwisko { get; set; }
    }
}
