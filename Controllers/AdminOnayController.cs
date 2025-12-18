using FitnessCenterReservationSystem.Data;
using FitnessCenterReservationSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterReservationSystem.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminOnayController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public AdminOnayController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		// GET: Index – Onay bekleyen kullanıcıları listele
		public async Task<IActionResult> Index()
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

		// POST: Kullanıcı Onayla
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> KullaniciOnayla(string id)
		{
			if (string.IsNullOrEmpty(id)) return NotFound();

			var user = await _context.Users.FindAsync(id);
			if (user == null) return NotFound();

			user.Onaylandi = true;
			user.EmailConfirmed = true; // mail onayını burada set ediyoruz

			_context.Update(user);
			await _context.SaveChangesAsync();

			TempData["Success"] = $"{user.Ad} {user.Soyad} başarıyla onaylandı.";
			return RedirectToAction(nameof(Index));
		}

		// POST: Kullanıcı Reddet
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> KullaniciReddet(string id)
		{
			if (string.IsNullOrEmpty(id)) return NotFound();

			var user = await _context.Users.FindAsync(id);
			if (user == null) return NotFound();

			_context.Users.Remove(user);
			await _context.SaveChangesAsync();

			TempData["Success"] = $"{user.Ad} {user.Soyad} reddedildi ve silindi.";
			return RedirectToAction(nameof(Index));
		}
	}
}
