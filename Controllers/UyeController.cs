using FitnessCenterReservationSystem.Data;
using FitnessCenterReservationSystem.Extensions;
using FitnessCenterReservationSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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


		// =====================================================
		// TÜM RANDEVULAR
		// =====================================================
		public async Task<IActionResult> TumRandevular()
		{
			var uyeId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var randevular = await _context.Randevular
				.Where(r => r.UyeId == uyeId)
				.Include(r => r.Antrenor)
				.Include(r => r.Hizmet)
				.Include(r => r.Salon)
				.OrderByDescending(r => r.Tarih)
				.ToListAsync();

			var model = randevular.ToRandevularListViewModel("Üye");
			return View(model);
		}

		public async Task<IActionResult> BekleyenRandevular()
		{
			return await RandevuListele(RandevuDurum.Beklemede);
		}

		public async Task<IActionResult> OnaylananRandevular()
		{
			return await RandevuListele(RandevuDurum.Onaylandi);
		}

		public async Task<IActionResult> TamamlananRandevular()
		{
			return await RandevuListele(RandevuDurum.Tamamlandi);
		}

		public async Task<IActionResult> ReddedilenRandevular()
		{
			return await RandevuListele(RandevuDurum.Reddedildi);
		}

		public async Task<IActionResult> IptalEdilenRandevular()
		{
			return await RandevuListele(RandevuDurum.Iptal);
		}

		// =====================================================
		// ORTAK LİSTE METODU (TEMİZ KOD)
		// =====================================================
		private async Task<IActionResult> RandevuListele(RandevuDurum durum)
		{
			var uyeId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var randevular = await _context.Randevular
				.Where(r => r.UyeId == uyeId && r.Durum == durum)
				.Include(r => r.Antrenor)
				.Include(r => r.Hizmet)
				.Include(r => r.Salon)
				.OrderByDescending(r => r.Tarih)
				.ToListAsync();

			var model = randevular.ToRandevularListViewModel("Üye");
			return View(model);
		}
	}
}
