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

		// GET: Listeleme
		public async Task<IActionResult> Index()
		{
			var hizmetler = await _context.Hizmetler
				.Include(h => h.Salon)
				.ToListAsync();
			return View(hizmetler);
		}

		// GET: Detay
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) return NotFound();

			var hizmet = await _context.Hizmetler
				.Include(h => h.Salon)
				.FirstOrDefaultAsync(h => h.Id == id);

			if (hizmet == null) return NotFound();

			return View(hizmet);
		}

		// GET: Yeni Hizmet
	 
		public IActionResult Create()
		{
			var salonlar = _context.Salonlar.ToList(); // Boş olsa bile hata vermez
			ViewBag.Salonlar = new SelectList(salonlar, "Id", "Ad"); // 0 yerine seçili yok
			return View();
		}
 

		// POST: Yeni Hizmet
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

		// GET: Düzenle
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) return NotFound();

			var hizmet = await _context.Hizmetler.FindAsync(id);
			if (hizmet == null) return NotFound();

			ViewBag.Salonlar = new SelectList(_context.Salonlar, "Id", "Ad", hizmet.SalonId);
			return View(hizmet);
		}

		// POST: Düzenle
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Hizmet hizmet)
		{
			if (id != hizmet.Id) return NotFound();

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(hizmet);
					await _context.SaveChangesAsync();
					TempData["Success"] = "Hizmet başarıyla güncellendi.";
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!_context.Hizmetler.Any(h => h.Id == id)) return NotFound();
					else throw;
				}
				return RedirectToAction(nameof(Index));
			}

			ViewBag.Salonlar = new SelectList(_context.Salonlar, "Id", "Ad", hizmet.SalonId);
			return View(hizmet);
		}

		// GET: Sil
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();

			var hizmet = await _context.Hizmetler
				.Include(h => h.Salon)
				.FirstOrDefaultAsync(h => h.Id == id);

			if (hizmet == null) return NotFound();

			return View(hizmet);
		}

		// POST: Sil
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var hizmet = await _context.Hizmetler.FindAsync(id);
			if (hizmet != null)
			{
				_context.Hizmetler.Remove(hizmet);
				await _context.SaveChangesAsync();
				TempData["Success"] = "Hizmet başarıyla silindi.";
			}

			return RedirectToAction(nameof(Index));
		}
	}
}
