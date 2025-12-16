using FitnessCenterReservationSystem.Data;
using FitnessCenterReservationSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterReservationSystem.Controllers
{
	[Authorize(Roles = "Üye")]
	public class UyeController : Controller
	{
		private readonly ApplicationDbContext _context;

		public UyeController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var userId = User.Identity.Name; // veya claim ile Id

			// Aktif randevular
			var activeAppointments = await _context.Randevular
				.Where(r => r.UyeId == userId && r.Tarih >= DateTime.Now)
				.ToListAsync();

			// Katıldığı hizmet sayısı
			var servicesCount = await _context.Randevular
				.Where(r => r.UyeId == userId)
				.Select(r => r.HizmetId)
				.Distinct()
				.CountAsync();

			// Mevcut kampanyalar
			var currentCampaigns = await _context.Kampanyalar
				.Where(k => k.BaslangicTarihi <= DateTime.Now && k.BitisTarihi >= DateTime.Now)
				.CountAsync();

			// Yaklaşan randevular (top 5)
			var upcomingAppointments = await _context.Randevular
				.Include(r => r.Hizmet)
				.Where(r => r.UyeId == userId && r.Tarih >= DateTime.Now)
				.OrderBy(r => r.Tarih)
				.Take(5)
				.ToListAsync();

			// Favori salonlar (örnek)
			var favoriteSalons = await _context.Salonlar
				.Take(5) // Basit örnek: en popüler 5
				.ToListAsync();

			ViewBag.ActiveAppointments = activeAppointments.Count;
			ViewBag.ServicesCount = servicesCount;
			ViewBag.CurrentCampaigns = currentCampaigns;
			ViewBag.UpcomingAppointments = upcomingAppointments;
			ViewBag.FavoriteSalons = favoriteSalons;

			return View();
		}
	}
}
