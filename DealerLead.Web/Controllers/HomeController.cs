using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DealerLead.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DealerLead.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DealerLeadDbContext _context;

        public HomeController(ILogger<HomeController> logger, DealerLeadDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            string oidString = GetOid();

            if (oidString != null)
            {
                ViewBag.ShowRegisterButton = true;
                var data = (from u in _context.DealerLeadUser
                           where u.AzureAdId == new Guid(oidString)
                           select u).ToList();

                if (data.Count == 0)
                {
                    ViewBag.ShowRegisterButton = true;
                    ViewBag.Registered = false;
                }
                else
                {
                    ViewBag.Registered = true;
                    ViewBag.ShowRegisterButton = false;
                }


            }
            else
            {
                ViewBag.Registered = false;
                ViewBag.ShowRegisterButton = false;
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Register()
        {
            DealerLeadUser newUser = new DealerLeadUser();

            newUser.AzureAdId = new Guid(GetOid());

            _context.DealerLeadUser.Add(newUser);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private string GetOid()
        {
            var user = this.User;
 
            if (user.Claims.ToList().Count > 0)
            {
                return user.Claims.ToList()[2].Value;
            }
            return null;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
