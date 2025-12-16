using FitnessCenterReservationSystem.Data;
using FitnessCenterReservationSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterReservationSystem.Controllers
{
	[Authorize(Roles = "Admin")]
	public class HizmetController : Controller
	{
		private readonly ApplicationDbContext _context;

		public HizmetController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Hizmet
		public async Task<IActionResult> Index()
		{
			var hizmetler = await _context.Hizmetler
				.Include(h => h.Salon)
				.ToListAsync();

			return View(hizmetler);
		}

		// GET: Hizmet/Create
		public async Task<IActionResult> Create()
		{
			ViewBag.Salonlar = new SelectList(
				await _context.Salonlar.ToListAsync(),
				"Id",
				"Ad"
			);

			return View();
		}

		// POST: Hizmet/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Hizmet hizmet)
		{
			if (ModelState.IsValid)
			{
				_context.Hizmetler.Add(hizmet);
				await _context.SaveChangesAsync();
				TempData["Success"] = "Hizmet başarıyla eklendi.";
				return RedirectToAction(nameof(Index));
			}

			ViewBag.Salonlar = new SelectList(_context.Salonlar, "Id", "Ad", hizmet.SalonId);
			return View(hizmet);
		}

		// GET: Hizmet/Edit/5
		public async Task<IActionResult> Edit(int id)
		{
			var hizmet = await _context.Hizmetler.FindAsync(id);
			if (hizmet == null) return NotFound();

			ViewBag.Salonlar = new SelectList(
				_context.Salonlar,
				"Id",
				"Ad",
				hizmet.SalonId
			);

			return View(hizmet);
		}

		// POST: Hizmet/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Hizmet hizmet)
		{
			if (id != hizmet.Id) return NotFound();

			if (ModelState.IsValid)
			{
				_context.Update(hizmet);
				await _context.SaveChangesAsync();
				TempData["Success"] = "Hizmet güncellendi.";
				return RedirectToAction(nameof(Index));
			}

			ViewBag.Salonlar = new SelectList(_context.Salonlar, "Id", "Ad", hizmet.SalonId);
			return View(hizmet);
		}

		// GET: Hizmet/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			var hizmet = await _context.Hizmetler
				.Include(h => h.Salon)
				.FirstOrDefaultAsync(h => h.Id  == id);

			if (hizmet == null) return NotFound();

			return View(hizmet);
		}

		// POST: Hizmet/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var hizmet = await _context.Hizmetler.FindAsync(id);
			if (hizmet != null)
				_context.Hizmetler.Remove(hizmet);

			await _context.SaveChangesAsync();
			TempData["Success"] = "Hizmet silindi.";
			return RedirectToAction(nameof(Index));
		}
	}
}
