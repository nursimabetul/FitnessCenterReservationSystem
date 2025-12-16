using FitnessCenterReservationSystem.Data;
using FitnessCenterReservationSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterReservationSystem.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		private readonly ApplicationDbContext _context;

		public AdminController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			// Toplam sayılar
			ViewBag.SalonCount = await _context.Salonlar.CountAsync();
			ViewBag.HizmetCount = await _context.Hizmetler.CountAsync();
			ViewBag.KampanyaCount = await _context.Kampanyalar.CountAsync();
			ViewBag.RandevuCount = await _context.Randevular.CountAsync();

			// Son eklenenler (örnek: son 5)
			ViewBag.RecentTrainers = await _context.Users
				.OrderByDescending(u => u.Id)
				.Take(5)
				.ToListAsync();

			ViewBag.RecentSalons = await _context.Salonlar
				.OrderByDescending(s => s.Id)
				.Take(5)
				.ToListAsync();

			return View();
		}
	}
}
