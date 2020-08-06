using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZEM_Enterprice_WebApp.Data;
using ZEM_Enterprice_WebApp.Utilities;

namespace ZEM_Enterprice_WebApp.API
{
    [Route("api/auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<MyUser> _userManager;
        private readonly SignInManager<MyUser> _signInManager;
        private readonly ILogger _logger;

        public AuthController(ApplicationDbContext db, UserManager<MyUser> userManager, SignInManager<MyUser> signInManager, ILogger<AuthController> logger)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        /// Returns loging in status for given credentials 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet("{username},{password}")]
        public async Task<ActionResult<bool>> GetUserAuth(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                _logger.LogInformation($"Failed to Auth user {username} with {password}");
                return false;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.Succeeded && (await
                _userManager.IsInRoleAsync(user, DefaultRoles.Scanner.ToString()) || await _userManager.IsInRoleAsync(user, DefaultRoles.Admin.ToString())))
                return true;
            return false;
        }
    }
}
