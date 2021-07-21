﻿using System;
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
            ViewBag.States = _context.SupportedState.ToList();

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

            ViewBag.States = _context.SupportedState.ToList();

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
    }
}
