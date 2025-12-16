using FitnessCenterReservationSystem.Data;
using FitnessCenterReservationSystem.Models;
using FitnessCenterReservationSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterReservationSystem.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AntrenorUzmanlikController : Controller
	{
		private readonly ApplicationDbContext _context;

		public AntrenorUzmanlikController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Listeleme
		public async Task<IActionResult> Index()
		{
			// "Antrenör" rolünü al
			var antrenorRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Antrenör");
			if (antrenorRole == null)
				return NotFound("Antrenör rolü bulunamadı.");

			// Antrenör kullanıcıları al, uzmanlıklarıyla birlikte
			var data = await _context.Users
				.Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == antrenorRole.Id))
				.Include(u => u.AntrenorUzmanlikAlanlari)
					.ThenInclude(a => a.UzmanlikAlani)
				.ToListAsync();

			return View(data);
		}

		// GET: Edit
		public async Task<IActionResult> Edit(string id)
		{
			var antrenor = await _context.Users
				.Include(u => u.AntrenorUzmanlikAlanlari)
				.FirstOrDefaultAsync(u => u.Id == id);

			if (antrenor == null)
				return NotFound();

			var tumUzmanliklar = await _context.UzmanlikAlanlari.ToListAsync();

			var model = new AntrenorUzmanlikViewModel
			{
				AntrenorId = antrenor.Id,
				AdSoyad = antrenor.Ad + " " + antrenor.Soyad,
				Email = antrenor.Email,
				TumUzmanliklar = tumUzmanliklar.Select(u => new UzmanlikCheckboxVM
				{
					Id = u.Id,
					Ad = u.Ad,
					Secili = antrenor.AntrenorUzmanlikAlanlari
						.Any(x => x.UzmanlikAlaniId == u.Id)
				}).ToList()
			};

			return View(model); // ✅
		}


		// POST: Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, int[] secilenUzmanlikler)
		{
			var antrenor = await _context.Users
				.Include(u => u.AntrenorUzmanlikAlanlari)
				.FirstOrDefaultAsync(u => u.Id == id);

			if (antrenor == null) return NotFound();

			// Mevcutları temizle
			var mevcutler = _context.AntrenorUzmanlikAlanlari.Where(a => a.AntrenorId == id);
			_context.AntrenorUzmanlikAlanlari.RemoveRange(mevcutler);

			// Yeni seçilenleri ekle
			foreach (var uzmanlikId in secilenUzmanlikler)
			{
				_context.AntrenorUzmanlikAlanlari.Add(new AntrenorUzmanlik
				{
					AntrenorId = id,
					UzmanlikAlaniId = uzmanlikId
				});
			}

			await _context.SaveChangesAsync();
			TempData["Success"] = "Uzmanlık alanları başarıyla güncellendi.";

			return RedirectToAction(nameof(Index));
		}
	}
}
