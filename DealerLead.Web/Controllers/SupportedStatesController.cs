using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace DealerLead.Web.Controllers
{
    [Authorize]
    public class SupportedStatesController : Controller
    {
        private readonly DealerLeadDbContext _context;
        private IMemoryCache _cache;

        public SupportedStatesController(DealerLeadDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: SupportedStates
        public async Task<IActionResult> Index()
        {
            return View(await _context.SupportedState.ToListAsync());
        }

        // GET: SupportedStates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportedState = await _context.SupportedState
                .FirstOrDefaultAsync(m => m.id == id);
            if (supportedState == null)
            {
                return NotFound();
            }

            return View(supportedState);
        }

        // GET: SupportedStates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SupportedStates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("id,Abbreviation,Name")] SupportedState supportedState)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supportedState);
                _context.SaveChanges();
                _cache.Set("states", _context.SupportedState.ToList());
                return RedirectToAction(nameof(Index));
            }
            return View(supportedState);
        }

        // GET: SupportedStates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportedState = await _context.SupportedState.FindAsync(id);
            if (supportedState == null)
            {
                return NotFound();
            }
            return View(supportedState);
        }

        // POST: SupportedStates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Abbreviation,Name")] SupportedState supportedState)
        {
            if (id != supportedState.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supportedState);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupportedStateExists(supportedState.id))
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
            return View(supportedState);
        }

        // GET: SupportedStates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportedState = await _context.SupportedState
                .FirstOrDefaultAsync(m => m.id == id);
            if (supportedState == null)
            {
                return NotFound();
            }

            return View(supportedState);
        }

        // POST: SupportedStates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supportedState = await _context.SupportedState.FindAsync(id);
            _context.SupportedState.Remove(supportedState);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupportedStateExists(int id)
        {
            return _context.SupportedState.Any(e => e.id == id);
        }
    }
}
