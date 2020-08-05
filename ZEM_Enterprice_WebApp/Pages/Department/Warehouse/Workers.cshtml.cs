using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZEM_Enterprice_WebApp.Data;
using ZEM_Enterprice_WebApp.Data.Tables;
using ZEM_Enterprice_WebApp.Utilities;

namespace ZEM_Enterprice_WebApp.Pages.Department.Warehouse
{
    [Authorize(Policy = "AdminOrMagazyn")]
    public class WorkersModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<MyUser> _userManager;

        public WorkersModel(ApplicationDbContext db, UserManager<MyUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Data od")]
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Data do")]
        [DataType(DataType.Date)]
        public DateTime DateTo { get; set; }


        public List<UserData> userDatas = new List<UserData>();
        public class UserData
        {
            public string User { get; set; }
            public int ScansDone { get; set; }
            public DateTime dataSkanowania { get; set; }
        }
        public async Task OnGetAsync()
        {
            DateFrom = DateTime.Now.Date;
            DateTo = DateTime.Now.Date;


            return;
        }
        public async Task<IActionResult> OnPostUpdateDataAsync()
        {
            return Page();
        }

        public StringBuilder CreateFile(bool shortDate = true)
        {
            StringBuilder writer = new StringBuilder();
            writer.AppendLine("Pracownik,Zeskanowane,Data");
            foreach (var entry in userDatas)
            {
                if(shortDate)
                    writer.AppendLine($"{entry.User},{entry.ScansDone},{entry.dataSkanowania.ToShortDateString()}");
                else
                    writer.AppendLine($"{entry.User},{entry.ScansDone},{entry.dataSkanowania.ToString()}");
            }

            return writer;
        }

        public async Task<IActionResult> OnPostDownloadHourDataAsync()
        {
            var users = await _userManager.GetUsersInRoleAsync(DefaultRoles.Scanner.ToString());
            foreach (var user in users)
            {
                var scansForUser = _db.VTMagazyn.Where(c =>
                        c.Pracownik == user.UserName
                        && c.DataUtworzenia.Date >= DateFrom.Date
                        && c.DataUtworzenia.Date <= DateTo.Date).OrderBy(c => c.DataUtworzenia).ToList();

                foreach (var scanPerUser in scansForUser)
                {
                    userDatas.Add(new UserData
                    {
                        User = user.UserName,
                        dataSkanowania = scanPerUser.DataUtworzenia
                    });
                }
            }

            return File(Encoding.UTF8.GetBytes(CreateFile(false).ToString()), "text/plain", $"pracownicy-godziny-{DateFrom.Day}-{DateFrom.Month}-{DateFrom.Year}-{DateTo.Day}-{DateTo.Month}-{DateTo.Year}.csv");
        }

        public async Task<IActionResult> OnPostDownloadDataAsync()
        {
            var users = await _userManager.GetUsersInRoleAsync(DefaultRoles.Scanner.ToString());
            foreach (var user in users)
            {
                var scansForUser = _db.VTMagazyn.Where(c =>
                        c.Pracownik == user.UserName
                        && c.DataUtworzenia.Date >= DateFrom.Date
                        && c.DataUtworzenia.Date <= DateTo.Date).OrderBy(c => c.DataUtworzenia.Date).ToList()
                        .GroupBy(c => c.DataUtworzenia.Date).Select(g => g.ToList()).ToList();

                foreach (var scanPerDate in scansForUser)
                {
                    userDatas.Add(new UserData
                    {
                        User = user.UserName,
                        ScansDone = scanPerDate.Count(),
                        dataSkanowania = scanPerDate[0].DataUtworzenia
                    });
                }
            }

            return File(Encoding.UTF8.GetBytes(CreateFile().ToString()), "text/plain", $"pracownicy-{DateFrom.Day}-{DateFrom.Month}-{DateFrom.Year}-{DateTo.Day}-{DateTo.Month}-{DateTo.Year}.csv");
        }
    }
}