using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace DealerLead.Web.Controllers
{
   

    public class DealershipsController : Controller
    {
        private readonly DealerLeadDbContext _context;
        private IMemoryCache _cache;
        
        public DealershipsController(DealerLeadDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public IActionResult Index()
        {
            var dealerships = _context.Dealership.ToList();
            ViewBag.States = GetStates();

            return View(dealerships);
        }

        public IActionResult Create()
        {
            var states = GetStates();

            return View(states);
        }

        [HttpPost]
        public IActionResult Create(Dealership dealership, int stateId)
        {

            dealership.CreatingUserId = GetUserId();
            dealership.State = stateId;

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

            var dealership = GetDealership((int)id);
            var state = GetState(dealership, (int)id);

            ViewBag.State = state;

            return View(dealership);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dealership = GetDealership((int)id);

            ViewBag.States = GetStates();

            return View(dealership);
        }

        [HttpPost]
        public IActionResult Edit(Dealership dealership, int id, int stateId)
        {
            _context.Update(dealership);
            dealership.ModifyDate = DateTime.Now;
            dealership.State = stateId;

            _context.SaveChanges();

            return RedirectToAction("Details", new { id = dealership.DealershipId });
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dealership = GetDealership((int) id);

            var state = GetState(dealership, (int)id);

            ViewBag.State = state;

            return View(dealership);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var dealership = GetDealership(id);

            _context.Remove(dealership);
            _context.SaveChanges();
            
            return RedirectToAction("Index");
        }

        public Dealership GetDealership(int id)
        {
            var dealership = (from d in _context.Dealership
                              where d.DealershipId == id
                              select d).FirstOrDefault();

            return dealership;
        }

        public SupportedState GetState(Dealership dealership, int id)
        {
            var state = (from s in _context.SupportedState
                         where dealership.State == s.id
                         select s).FirstOrDefault();

            return state;
        }

        public List<SupportedState> GetStates()
        {

            if (!_cache.TryGetValue("states", out var data))
            {
                // Key not in cache, so get data.
                data = _context.SupportedState.ToList();
                // Save data in cache and set the relative expiration time
                _cache.Set("states", data, TimeSpan.FromMinutes(60));
            }

            return data as List<SupportedState>;
        }

        public int GetUserId()
        {
            if (HttpContext.Session.TryGetValue("userId", out var userId) == false)
            {
                var result = (from u in _context.DealerLeadUser
                              where u.AzureAdId == IdentityHelper.GetAzureOIDToken(this.User)
                              select u.UserId).FirstOrDefault();

                userId = BitConverter.GetBytes(result);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(userId);

                HttpContext.Session.Set("userId", userId);
            }

            return BitConverter.ToInt32(userId);
        }
    }
}
