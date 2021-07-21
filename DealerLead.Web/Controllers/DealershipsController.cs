using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DealerLead.Web.Controllers
{
    public class DealershipsController : Controller
    {
        private readonly DealerLeadDbContext _context;

        public DealershipsController(DealerLeadDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var dealerships = _context.Dealership.ToList();

            return View(dealerships);
        }

        public IActionResult Create()
        {
            var states = _context.SupportedState.ToList(); 
            
            return View(states);
        }

        [HttpPost]
        public IActionResult Create(Dealership dealership, int id)
        {
            var userId = (from u in _context.DealerLeadUser
                          where u.AzureAdId == IdentityHelper.GetAzureOIDToken(this.User)
                          select u.UserId).FirstOrDefault();

            dealership.CreatingUserId = userId;
            dealership.State = id;

            _context.Dealership.Add(dealership);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dealership = (from d in _context.Dealership
                             where d.DealershipId == id
                             select d).FirstOrDefault();

            var state = (from s in _context.SupportedState
                         where dealership.State == s.id
                         select s).FirstOrDefault();

            ViewBag.State = state;

            return View(dealership);
        }

    }
}
