using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DealerLead;
using Microsoft.AspNetCore.Authorization;

namespace DealerLead.Web.Controllers
{
    [Authorize]
    public class SupportedModelsController : Controller
    {
        private readonly DealerLeadDbContext _context;

        public SupportedModelsController(DealerLeadDbContext context)
        {
            _context = context;
        }

        // GET: SupportedModels
        public async Task<IActionResult> Index()
        {
            var dealerLeadDbContext = _context.SupportedModel.Include(s => s.Make);
            return View(await dealerLeadDbContext.ToListAsync());
        }

        // GET: SupportedModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportedModel = await _context.SupportedModel
                .Include(s => s.Make)
                .FirstOrDefaultAsync(m => m.ModelId == id);
            if (supportedModel == null)
            {
                return NotFound();
            }

            return View(supportedModel);
        }

        // GET: SupportedModels/Create
        public IActionResult Create()
        {
            ViewData["MakeId"] = new SelectList(_context.SupportedMake, "MakeId", "Name");
            return View();
        }

        // POST: SupportedModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModelId,ModelName,MakeId")] SupportedModel supportedModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supportedModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MakeId"] = new SelectList(_context.SupportedMake, "MakeId", "Name", supportedModel.MakeId);
            return View(supportedModel);
        }

        // GET: SupportedModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportedModel = await _context.SupportedModel.FindAsync(id);
            if (supportedModel == null)
            {
                return NotFound();
            }
            ViewData["MakeId"] = new SelectList(_context.SupportedMake, "MakeId", "Name", supportedModel.MakeId);
            return View(supportedModel);
        }

        // POST: SupportedModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModelId,ModelName,MakeId")] SupportedModel supportedModel)
        {
            if (id != supportedModel.ModelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supportedModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupportedModelExists(supportedModel.ModelId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MakeId"] = new SelectList(_context.SupportedMake, "MakeId", "Name", supportedModel.MakeId);
            return View(supportedModel);
        }

        // GET: SupportedModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportedModel = await _context.SupportedModel
                .Include(s => s.Make)
                .FirstOrDefaultAsync(m => m.ModelId == id);
            if (supportedModel == null)
            {
                return NotFound();
            }

            return View(supportedModel);
        }

        // POST: SupportedModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supportedModel = await _context.SupportedModel.FindAsync(id);
            _context.SupportedModel.Remove(supportedModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupportedModelExists(int id)
        {
            return _context.SupportedModel.Any(e => e.ModelId == id);
        }
    }
}
