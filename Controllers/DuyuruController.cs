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
    public class DuyuruController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DuyuruController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Duyuru
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Duyurular.Include(d => d.Olusturan).Include(d => d.Salon);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Duyuru/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var duyuru = await _context.Duyurular
                .Include(d => d.Olusturan)
                .Include(d => d.Salon)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (duyuru == null)
            {
                return NotFound();
            }

            return View(duyuru);
        }

        // GET: Duyuru/Create
        public IActionResult Create()
        {
            ViewData["OlusturanId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["SalonId"] = new SelectList(_context.Salonlar, "Id", "Ad");
            return View();
        }

        // POST: Duyuru/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Baslik,Icerik,Tarih,SalonId,OlusturanId")] Duyuru duyuru)
        {
            if (ModelState.IsValid)
            {
                _context.Add(duyuru);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OlusturanId"] = new SelectList(_context.Users, "Id", "Id", duyuru.OlusturanId);
            ViewData["SalonId"] = new SelectList(_context.Salonlar, "Id", "Ad", duyuru.SalonId);
            return View(duyuru);
        }

        // GET: Duyuru/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var duyuru = await _context.Duyurular.FindAsync(id);
            if (duyuru == null)
            {
                return NotFound();
            }
            ViewData["OlusturanId"] = new SelectList(_context.Users, "Id", "Id", duyuru.OlusturanId);
            ViewData["SalonId"] = new SelectList(_context.Salonlar, "Id", "Ad", duyuru.SalonId);
            return View(duyuru);
        }

        // POST: Duyuru/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Baslik,Icerik,Tarih,SalonId,OlusturanId")] Duyuru duyuru)
        {
            if (id != duyuru.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(duyuru);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DuyuruExists(duyuru.Id))
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
            ViewData["OlusturanId"] = new SelectList(_context.Users, "Id", "Id", duyuru.OlusturanId);
            ViewData["SalonId"] = new SelectList(_context.Salonlar, "Id", "Ad", duyuru.SalonId);
            return View(duyuru);
        }

        // GET: Duyuru/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var duyuru = await _context.Duyurular
                .Include(d => d.Olusturan)
                .Include(d => d.Salon)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (duyuru == null)
            {
                return NotFound();
            }

            return View(duyuru);
        }

        // POST: Duyuru/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var duyuru = await _context.Duyurular.FindAsync(id);
            if (duyuru != null)
            {
                _context.Duyurular.Remove(duyuru);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DuyuruExists(int id)
        {
            return _context.Duyurular.Any(e => e.Id == id);
        }
    }
}
