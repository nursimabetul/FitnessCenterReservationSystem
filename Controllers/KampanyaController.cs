using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessCenterReservationSystem.Data;
using FitnessCenterReservationSystem.Models;

namespace FitnessCenterReservationSystem.Controllers
{
    public class KampanyaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KampanyaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Kampanya
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Kampanyalar.Include(k => k.Hizmet);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Kampanya/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kampanya = await _context.Kampanyalar
                .Include(k => k.Hizmet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kampanya == null)
            {
                return NotFound();
            }

            return View(kampanya);
        }

        // GET: Kampanya/Create
        public IActionResult Create()
        {
			ViewBag.SalonId = new SelectList(_context.Salonlar, "Id", "Ad");
			ViewBag.HizmetId = new SelectList(_context.Hizmetler, "Id", "Ad");
			return View();
        }

        // POST: Kampanya/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Baslik,Aciklama,BaslangicTarihi,BitisTarihi,SalonId,HizmetId")] Kampanya kampanya)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kampanya);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HizmetId"] = new SelectList(_context.Hizmetler, "Id", "Ad", kampanya.HizmetId);
            return View(kampanya);
        }

        // GET: Kampanya/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kampanya = await _context.Kampanyalar.FindAsync(id);
            if (kampanya == null)
            {
                return NotFound();
            }
            ViewData["HizmetId"] = new SelectList(_context.Hizmetler, "Id", "Ad", kampanya.HizmetId);
            return View(kampanya);
        }

        // POST: Kampanya/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Baslik,Aciklama,BaslangicTarihi,BitisTarihi,SalonId,HizmetId")] Kampanya kampanya)
        {
            if (id != kampanya.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kampanya);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KampanyaExists(kampanya.Id))
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
            ViewData["HizmetId"] = new SelectList(_context.Hizmetler, "Id", "Ad", kampanya.HizmetId);
            return View(kampanya);
        }

        // GET: Kampanya/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kampanya = await _context.Kampanyalar
                .Include(k => k.Hizmet)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kampanya == null)
            {
                return NotFound();
            }

            return View(kampanya);
        }

        // POST: Kampanya/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kampanya = await _context.Kampanyalar.FindAsync(id);
            if (kampanya != null)
            {
                _context.Kampanyalar.Remove(kampanya);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KampanyaExists(int id)
        {
            return _context.Kampanyalar.Any(e => e.Id == id);
        }
    }
}
