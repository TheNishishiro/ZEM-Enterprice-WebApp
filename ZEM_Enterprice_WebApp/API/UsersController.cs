using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZEM_Enterprice_WebApp.Data;
using ZEM_Enterprice_WebApp.Data.Tables;
using ZEM_Enterprice_WebApp.Pages.Department.Scanner;
using ZEM_Enterprice_WebApp.Utilities;

namespace ZEM_Enterprice_WebApp.API
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<MyUser> _userManager;

        public UsersController(ApplicationDbContext db, UserManager<MyUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        /// <summary>
        /// Returns scans conducted by each worker
        /// </summary>
        /// <param name="day1"></param>
        /// <param name="month1"></param>
        /// <param name="year1"></param>
        /// <param name="day2"></param>
        /// <param name="month2"></param>
        /// <param name="year2"></param>
        /// <returns></returns>
        [HttpGet("{day1},{month1},{year1},{day2},{month2},{year2}")]
        public async Task<ActionResult<IEnumerable<UserData>>> GetUserPerformence(int day1, int month1, int year1, int day2, int month2, int year2)
        {
            List<UserData> userData = new List<UserData>();

            DateTime dateFrom = new DateTime(year1, month1, day1);
            DateTime dateTo = new DateTime(year2, month2, day2);

            var users = await _userManager.GetUsersInRoleAsync(DefaultRoles.Scanner.ToString());
            foreach (var user in users)
            {
                userData.Add(new UserData
                {
                    User = user.UserName,
                    ScansDone = _db.VTMagazyn.Where(c => 
                        c.Pracownik == user.UserName 
                        && c.DataUtworzenia.Date >= dateFrom.Date 
                        && c.DataUtworzenia.Date <= dateTo.Date).Count()
                });
            }
            return userData;
        }
    }

    [Route("api/userstime")]
    [ApiController]
    public class UsersTimeController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<MyUser> _userManager;

        public UsersTimeController(ApplicationDbContext db, UserManager<MyUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        /// <summary>
        /// Returns scans conducted by each worker per day
        /// </summary>
        /// <param name="day1"></param>
        /// <param name="month1"></param>
        /// <param name="year1"></param>
        /// <param name="day2"></param>
        /// <param name="month2"></param>
        /// <param name="year2"></param>
        /// <returns></returns>
        [HttpGet("{day1},{month1},{year1},{day2},{month2},{year2}")]
        public async Task<ActionResult<IEnumerable<UserTimeSeries>>> GetUserPerformence(int day1, int month1, int year1, int day2, int month2, int year2)
        {
            List<UserTimeSeries> timeSeries = new List<UserTimeSeries>();

            DateTime dateFrom = new DateTime(year1, month1, day1);
            DateTime dateTo = new DateTime(year2, month2, day2);
            var dates = Enumerable.Range(0, 1 + dateTo.Subtract(dateFrom).Days)
              .Select(offset => dateFrom.AddDays(offset))
              .ToArray();

            var users = await _userManager.GetUsersInRoleAsync(DefaultRoles.Scanner.ToString());
            foreach (var user in users)
            {

                UserTimeSeries UTS = new UserTimeSeries();

                UTS.User = user.UserName;
                UTS.Dates = new List<string>();
                UTS.Values = new List<int>();

                foreach (var date in dates)
                {
                    UTS.Dates.Add(date.ToShortDateString());
                    UTS.Values.Add(_db.VTMagazyn.Where(c => c.Pracownik == user.UserName && c.DataUtworzenia.Date == date.Date).Count());
                }

                timeSeries.Add(UTS);
            }
            return timeSeries;
        }
    }
    public class UserTimeSeries
    {
        public string User { get; set; }
        public List<string> Dates { get; set; }
        public List<int> Values { get; set; }
    }
    public class UserData
    {
        public string User { get; set; }
        public int ScansDone { get; set; }
    }
}
