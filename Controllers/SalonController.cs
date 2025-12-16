using FitnessCenterReservationSystem.Data;
using FitnessCenterReservationSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterReservationSystem.Controllers
{
	[Authorize(Roles = "Admin")]
	public class SalonController : Controller
	{
		private readonly ApplicationDbContext _context;

		public SalonController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Salon
		public async Task<IActionResult> Index()
		{
			return View(await _context.Salonlar.ToListAsync());
		}

		// GET: Salon/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) return NotFound();

			var salon = await _context.Salonlar
				.Include(s => s.Hizmetler)
				.FirstOrDefaultAsync(s => s.Id == id);

			if (salon == null) return NotFound();

			return View(salon);
		}

		// GET: Salon/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Salon/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Salon salon)
		{
			if (!ModelState.IsValid)
				return View(salon);

			_context.Salonlar.Add(salon);
			await _context.SaveChangesAsync();

			TempData["Success"] = "Salon başarıyla eklendi.";
			return RedirectToAction(nameof(Index));
		}

		// GET: Salon/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) return NotFound();

			var salon = await _context.Salonlar.FindAsync(id);
			if (salon == null) return NotFound();

			return View(salon);
		}

		// POST: Salon/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Salon salon)
		{
			if (id != salon.Id) return NotFound();

			if (!ModelState.IsValid)
				return View(salon);

			try
			{
				_context.Update(salon);
				await _context.SaveChangesAsync();

				TempData["Success"] = "Salon bilgileri güncellendi.";
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!_context.Salonlar.Any(e => e.Id == salon.Id))
					return NotFound();
				else
					throw;
			}

			return RedirectToAction(nameof(Index));
		}

		// GET: Salon/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();

			var salon = await _context.Salonlar.FirstOrDefaultAsync(s => s.Id == id);
			if (salon == null) return NotFound();

			return View(salon);
		}

		// POST: Salon/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var salon = await _context.Salonlar
				.Include(s => s.Randevular)
				.Include(s => s.Hizmetler)
				.FirstOrDefaultAsync(s => s.Id == id);

			if (salon == null) return NotFound();

			// Güvenlik: bağlı veri varsa silme
			if (salon.Randevular.Any() || salon.Hizmetler.Any())
			{
				TempData["Error"] = "Bu salona bağlı randevu veya hizmetler var. Önce onları silmelisiniz.";
				return RedirectToAction(nameof(Index));
			}

			_context.Salonlar.Remove(salon);
			await _context.SaveChangesAsync();

			TempData["Success"] = "Salon başarıyla silindi.";
			return RedirectToAction(nameof(Index));
		}
	}
}
