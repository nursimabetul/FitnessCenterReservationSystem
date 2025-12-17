using FitnessCenterReservationSystem.Data;
using FitnessCenterReservationSystem.Models;
using FitnessCenterReservationSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterReservationSystem.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AntrenorCalismaSaatiController : Controller
	{
		private readonly ApplicationDbContext _context;

		public AntrenorCalismaSaatiController(ApplicationDbContext context)
		{
			_context = context;
		}

		// =============================
		// Index – Tüm çalışma saatlerini listele
		// =============================
		public async Task<IActionResult> Index()
		{
			var antrenorRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Antrenör");
			var calismaSaatleri = await _context.AntrenorCalismaSaatleri
				.Include(a => a.Antrenor)
				.Where(a => _context.UserRoles
					.Any(ur => ur.UserId == a.AntrenorId && ur.RoleId == antrenorRole.Id))
				.ToListAsync();

			return View(calismaSaatleri);
		}

		// =============================
		// Create – GET
		// =============================
		public IActionResult Create()
		{
			// 1. Antrenörleri çek
			var antrenorler = _context.Users
				.Where(u => _context.UserRoles
					.Any(ur => ur.UserId == u.Id &&
							   _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == "Antrenör")))
				.Select(u => new { u.Id, AdSoyad = u.Ad + " " + u.Soyad })
				.ToList();

			// 2. ViewData'ya gönder
			ViewData["AntrenorId"] = new SelectList(antrenorler, "Id", "AdSoyad");

			// 3. View'ı döndür
			return View();
		}

		// =============================
		// Create – POST
		// =============================
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("AntrenorId,Gun,BaslangicSaati,BitisSaati")] AntrenorCalismaSaati calismaSaati)
		{
			if (!ModelState.IsValid)
				return View(calismaSaati);

			// --- Çakışma kontrolü ---
			bool cakismaVar = _context.AntrenorCalismaSaatleri.Any(a =>
				a.AntrenorId == calismaSaati.AntrenorId &&
				a.Gun == calismaSaati.Gun &&
				((calismaSaati.BaslangicSaati >= a.BaslangicSaati && calismaSaati.BaslangicSaati < a.BitisSaati) ||
				 (calismaSaati.BitisSaati > a.BaslangicSaati && calismaSaati.BitisSaati <= a.BitisSaati) ||
				 (calismaSaati.BaslangicSaati <= a.BaslangicSaati && calismaSaati.BitisSaati >= a.BitisSaati))
			);

			if (cakismaVar)
			{
				ModelState.AddModelError("", "Bu saat aralığında zaten bir kayıt mevcut!");

				// --- Dropdown tekrar doldurulmalı ---
				var antrenorler = _context.Users
				   .Where(u => _context.UserRoles
					   .Any(ur => ur.UserId == u.Id &&
								  _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == "Antrenör")))
				   .Select(u => new { u.Id, AdSoyad = u.Ad + " " + u.Soyad })
				   .ToList();

				ViewData["AntrenorId"] = new SelectList(antrenorler, "Id", "AdSoyad", calismaSaati.AntrenorId);

				return View(calismaSaati);
			}

			_context.Add(calismaSaati);
			await _context.SaveChangesAsync();
			TempData["Success"] = "Çalışma saati başarıyla eklendi.";
			return RedirectToAction(nameof(Index));
		}


		// =============================
		// Edit – GET
		// =============================
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) return NotFound();

			var calismaSaati = await _context.AntrenorCalismaSaatleri.FindAsync(id);
			if (calismaSaati == null) return NotFound();

			var antrenorler = _context.Users
			.Where(u => _context.UserRoles
				.Any(ur => ur.UserId == u.Id &&
						   _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == "Antrenör")))
			.Select(u => new { u.Id, AdSoyad = u.Ad + " " + u.Soyad })
			.ToList();

			// 2. ViewData’ya gönder
			ViewData["AntrenorId"] = new SelectList(antrenorler, "Id", "AdSoyad", calismaSaati.AntrenorId);

			return View(calismaSaati);
		}

		// =============================
		// Edit – POST
		// =============================
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,AntrenorId,Gun,BaslangicSaati,BitisSaati")] AntrenorCalismaSaati calismaSaati)
		{
			if (id != calismaSaati.Id)
				return NotFound();

			if (!ModelState.IsValid)
			{
				// --- Dropdown tekrar doldur ---
				var antrenorler = _context.Users
					.Where(u => _context.UserRoles
						.Any(ur => ur.UserId == u.Id &&
								   _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == "Antrenör")))
					.Select(u => new { u.Id, AdSoyad = u.Ad + " " + u.Soyad })
					.ToList();

				ViewData["AntrenorId"] = new SelectList(antrenorler, "Id", "AdSoyad", calismaSaati.AntrenorId);

				return View(calismaSaati);
			}

			// --- Çakışma kontrolü ---
			bool cakismaVar = _context.AntrenorCalismaSaatleri.Any(a =>
				a.Id != calismaSaati.Id && // kendi kaydını kontrol etme
				a.AntrenorId == calismaSaati.AntrenorId &&
				a.Gun == calismaSaati.Gun &&
				((calismaSaati.BaslangicSaati >= a.BaslangicSaati && calismaSaati.BaslangicSaati < a.BitisSaati) ||
				 (calismaSaati.BitisSaati > a.BaslangicSaati && calismaSaati.BitisSaati <= a.BitisSaati) ||
				 (calismaSaati.BaslangicSaati <= a.BaslangicSaati && calismaSaati.BitisSaati >= a.BitisSaati))
			);

			if (cakismaVar)
			{
				ModelState.AddModelError("", "Bu saat aralığında zaten bir kayıt mevcut!");

				// --- Dropdown tekrar doldur ---
				var antrenorler = _context.Users
					.Where(u => _context.UserRoles
						.Any(ur => ur.UserId == u.Id &&
								   _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == "Antrenör")))
					.Select(u => new { u.Id, AdSoyad = u.Ad + " " + u.Soyad })
					.ToList();

				ViewData["AntrenorId"] = new SelectList(antrenorler, "Id", "AdSoyad", calismaSaati.AntrenorId);

				return View(calismaSaati);
			}

			try
			{
				_context.Update(calismaSaati);
				await _context.SaveChangesAsync();
				TempData["Success"] = "Çalışma saati başarıyla güncellendi.";
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!_context.AntrenorCalismaSaatleri.Any(e => e.Id == calismaSaati.Id))
					return NotFound();
				else
					throw;
			}

			return RedirectToAction(nameof(Index));
		}


		// =============================
		// Delete – GET
		// =============================
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();

			var calismaSaati = await _context.AntrenorCalismaSaatleri
				.Include(a => a.Antrenor)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (calismaSaati == null) return NotFound();

			return View(calismaSaati);
		}

		// =============================
		// Delete – POST
		// =============================
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var calismaSaati = await _context.AntrenorCalismaSaatleri.FindAsync(id);
			_context.AntrenorCalismaSaatleri.Remove(calismaSaati);
			await _context.SaveChangesAsync();
			TempData["Success"] = "Çalışma saati başarıyla silindi.";
			return RedirectToAction(nameof(Index));
		}
	}
}
