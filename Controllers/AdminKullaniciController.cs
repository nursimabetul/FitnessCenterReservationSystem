using FitnessCenterReservationSystem.Data;
using FitnessCenterReservationSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterReservationSystem.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminKullaniciController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public AdminKullaniciController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		// GET: Tüm kullanıcılar
		public async Task<IActionResult> Index()
		{
			var users = await _context.Users
				.Select(u => new
				{
					User = u,
					Role = _context.UserRoles
								.Where(ur => ur.UserId == u.Id)
								.Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
								.FirstOrDefault()
				})
				.ToListAsync();

			return View("KullaniciListe", users); // Ortak view
		}

		// GET: Onay Bekleyenler
		public async Task<IActionResult> OnayBekleyen()
		{
			var onayBekleyenKullanicilar = await _context.Users
				.Where(u => !u.Onaylandi)
				.Select(u => new
				{
					User = u,
					Role = _context.UserRoles
								.Where(ur => ur.UserId == u.Id)
								.Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
								.FirstOrDefault()
				})
				.ToListAsync();

			return View(onayBekleyenKullanicilar);
		}


		// GET: Aktif kullanıcılar
		public async Task<IActionResult> AktifKullanicilar()
		{
			var users = await _context.Users
				.Where(u => u.Onaylandi && u.AktifMi)
				.Select(u => new
				{
					User = u,
					Role = _context.UserRoles
								.Where(ur => ur.UserId == u.Id)
								.Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
								.FirstOrDefault()
				})
				.ToListAsync();

			return View("KullaniciListe", users);
		}

		// GET: Pasif / Reddedilen kullanıcılar
		public async Task<IActionResult> PasifKullanicilar()
		{
			var users = await _context.Users
				.Where(u => !u.Onaylandi || !u.AktifMi)
				.Select(u => new
				{
					User = u,
					Role = _context.UserRoles
								.Where(ur => ur.UserId == u.Id)
								.Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
								.FirstOrDefault()
				})
				.ToListAsync();

			return View("KullaniciListe", users);
		}

		// POST: Kullanıcı Onayla
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> KullaniciOnayla(string id)
		{
			if (string.IsNullOrEmpty(id)) return NotFound();

			var user = await _context.Users.FindAsync(id);
			if (user == null) return NotFound();

			user.Onaylandi = true;
			user.AktifMi = true;
			user.EmailConfirmed = true;

			_context.Update(user);
			await _context.SaveChangesAsync();

			TempData["Success"] = $"{user.Ad} {user.Soyad} başarıyla onaylandı.";
			return RedirectToAction(nameof(Index));
		}

		// POST: Kullanıcı Pasifleştir / Sil
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> KullaniciPasiflestir(string id)
		{
			if (string.IsNullOrEmpty(id)) return NotFound();

			var user = await _context.Users.FindAsync(id);
			if (user == null) return NotFound();

			user.AktifMi = false;
			_context.Update(user);
			await _context.SaveChangesAsync();

			TempData["Success"] = $"{user.Ad} {user.Soyad} pasif hale getirildi.";
			return RedirectToAction(nameof(PasifKullanicilar));
		}

		// POST: Kullanıcı Sil
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> KullaniciSil(string id)
		{
			if (string.IsNullOrEmpty(id)) return NotFound();

			var user = await _context.Users.FindAsync(id);
			if (user == null) return NotFound();

			_context.Users.Remove(user);
			await _context.SaveChangesAsync();

			TempData["Success"] = $"{user.Ad} {user.Soyad} silindi.";
			return RedirectToAction(nameof(PasifKullanicilar));
		}
	}
}
