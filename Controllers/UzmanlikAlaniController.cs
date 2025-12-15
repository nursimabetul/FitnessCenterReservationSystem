using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessCenterReservationSystem.Data;
using FitnessCenterReservationSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace FitnessCenterReservationSystem.Controllers
{
	[Authorize(Roles = "Admin")]
	public class UzmanlikAlaniController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UzmanlikAlaniController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UzmanlikAlani
        public async Task<IActionResult> Index()
        {
            return View(await _context.UzmanlikAlanlari.ToListAsync());
        }

        // GET: UzmanlikAlani/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uzmanlikAlani = await _context.UzmanlikAlanlari
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uzmanlikAlani == null)
            {
                return NotFound();
            }

            return View(uzmanlikAlani);
        }

        // GET: UzmanlikAlani/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UzmanlikAlani/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ad")] UzmanlikAlani uzmanlikAlani)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uzmanlikAlani);
                await _context.SaveChangesAsync();
				TempData["Success"] = "Uzmanlık alanı başarıyla eklendi.";

				return RedirectToAction(nameof(Index));
            }
            return View(uzmanlikAlani);
        }

        // GET: UzmanlikAlani/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uzmanlikAlani = await _context.UzmanlikAlanlari.FindAsync(id);
            if (uzmanlikAlani == null)
            {
                return NotFound();
            }
            return View(uzmanlikAlani);
        }

        // POST: UzmanlikAlani/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ad")] UzmanlikAlani uzmanlikAlani)
        {
            if (id != uzmanlikAlani.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uzmanlikAlani);
					TempData["Success"] = "Uzmanlık alanı başarıyla güncellendi.";

					await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UzmanlikAlaniExists(uzmanlikAlani.Id))
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
            return View(uzmanlikAlani);
        }

        // GET: UzmanlikAlani/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uzmanlikAlani = await _context.UzmanlikAlanlari
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uzmanlikAlani == null)
            {
                return NotFound();
            }

            return View(uzmanlikAlani);
        }

        // POST: UzmanlikAlani/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uzmanlikAlani = await _context.UzmanlikAlanlari.FindAsync(id);
            if (uzmanlikAlani != null)
            {
                _context.UzmanlikAlanlari.Remove(uzmanlikAlani);
            }

            await _context.SaveChangesAsync();
			TempData["Success"] = "Uzmanlık alanı başarıyla silindi.";

			return RedirectToAction(nameof(Index));
        }

        private bool UzmanlikAlaniExists(int id)
        {
            return _context.UzmanlikAlanlari.Any(e => e.Id == id);
        }
    }
}
