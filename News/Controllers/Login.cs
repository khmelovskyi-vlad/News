using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace News.Controllers
{
    public class Login : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Logout()
        {
            //await HttpContext.SignOutAsync("Cookies");
            //await HttpContext.SignOutAsync("oidc");
            return SignOut("Cookies", "oidc");
        }
    }
}
