using FitnessCenterReservationSystem.Data;
using FitnessCenterReservationSystem.Models;
using FitnessCenterReservationSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterReservationSystem.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AntrenorHizmetController : Controller
	{
		private readonly ApplicationDbContext _context;

		public AntrenorHizmetController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Liste
		public async Task<IActionResult> Index()
		{
			var antrenorRole = await _context.Roles
				.FirstOrDefaultAsync(r => r.Name == "Antrenör");

			if (antrenorRole == null)
				return NotFound("Antrenör rolü bulunamadı.");

			var antrenorler = await _context.Users
				.Where(u => _context.UserRoles
					.Any(ur => ur.UserId == u.Id && ur.RoleId == antrenorRole.Id))
				.Include(u => u.AntrenorHizmetler)
					.ThenInclude(ah => ah.Hizmet)
				.ToListAsync();

			return View(antrenorler);
		}

		// GET: Edit
		public async Task<IActionResult> Edit(string id)
		{
			if (id == null) return NotFound();

			var antrenor = await _context.Users
				.Include(u => u.AntrenorHizmetler)
				.FirstOrDefaultAsync(u => u.Id == id);

			if (antrenor == null) return NotFound();

			var tumHizmetler = await _context.Hizmetler.ToListAsync();

			var model = new AntrenorHizmetViewModel
			{
				AntrenorId = antrenor.Id,
				AdSoyad = antrenor.Ad + " " + antrenor.Soyad,
				Email = antrenor.Email,
				Hizmetler = tumHizmetler.Select(h => new HizmetCheckboxVM
				{
					Id = h.Id,
					Ad = h.Ad,
					Secili = antrenor.AntrenorHizmetler
						.Any(x => x.HizmetId == h.Id)
				}).ToList()
			};

			return View(model);
		}

		// POST: Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, int[] secilenHizmetler)
		{
			var antrenor = await _context.Users
				.Include(u => u.AntrenorHizmetler)
				.FirstOrDefaultAsync(u => u.Id == id);

			if (antrenor == null) return NotFound();

			// Mevcut ilişkileri sil
			var mevcutlar = _context.AntrenorHizmetler
				.Where(x => x.AntrenorId == id);
			_context.AntrenorHizmetler.RemoveRange(mevcutlar);

			// Yeni seçilenleri ekle
			foreach (var hizmetId in secilenHizmetler)
			{
				_context.AntrenorHizmetler.Add(new AntrenorHizmet
				{
					AntrenorId = id,
					HizmetId = hizmetId
				});
			}

			await _context.SaveChangesAsync();
			TempData["Success"] = "Antrenör hizmetleri başarıyla güncellendi.";

			return RedirectToAction(nameof(Index));
		}
	}
}
