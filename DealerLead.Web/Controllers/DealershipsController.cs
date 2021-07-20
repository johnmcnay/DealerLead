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
         
            
            return View();
        }

        [HttpPost]
        public IActionResult Create(Dealership dealership)
        {


            return RedirectToAction("Index");
        }

    }
}
