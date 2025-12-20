using FitnessCenterReservationSystem.Data;
using FitnessCenterReservationSystem.Extensions;
using FitnessCenterReservationSystem.Models;
using FitnessCenterReservationSystem.ViewModels;
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


		// DASHBOARD 
		// =====================================================
		public async Task<IActionResult> Index()
		{
			var today = DateTime.Today;

			// =====================================================
			// Süresi geçmiş onaylı randevuları tamamla
			// =====================================================
			var tamamlanacaklar = await _context.Randevular
				.Where(r => r.Durum == RandevuDurum.Onaylandi && r.Tarih < today)
				.ToListAsync();

			foreach (var r in tamamlanacaklar)
				r.Durum = RandevuDurum.Tamamlandi;

			await _context.SaveChangesAsync();

			// =====================================================
			// RANDEVU VERİLERİ
			// =====================================================
			var aktifRandevular = await _context.Randevular
				.Where(r => r.Tarih >= today
							&& r.Durum != RandevuDurum.Iptal
							&& r.Durum != RandevuDurum.Reddedildi)
				.ToListAsync();

			var yaklasan = aktifRandevular
				.OrderBy(r => r.Tarih)
				.Take(5)
				.ToList();

			var bugun = await _context.Randevular
				.Where(r => r.Tarih == today && r.Durum == RandevuDurum.Onaylandi)
				.Include(r => r.Uye)
				.Include(r => r.Antrenor)
				.Include(r => r.Hizmet)
				.Include(r => r.Salon)
				.ToListAsync();

			var model = new DashboardViewModel
			{
				AktifRandevular = aktifRandevular.Count,
				BugunRandevular = bugun.ToRandevularListViewModel("Admin"),
				BugunRandevuSayisi = bugun.Count, 
				YaklasanRandevular = yaklasan.ToRandevularListViewModel("Admin"),
				Bekleyen = await _context.Randevular.CountAsync(r => r.Durum == RandevuDurum.Beklemede),
				Onaylanan = await _context.Randevular.CountAsync(r => r.Durum == RandevuDurum.Onaylandi),
				Reddedilen = await _context.Randevular.CountAsync(r => r.Durum == RandevuDurum.Reddedildi),
				IptalEdilen = await _context.Randevular.CountAsync(r => r.Durum == RandevuDurum.Iptal),
				Tamamlanan = await _context.Randevular.CountAsync(r => r.Durum == RandevuDurum.Tamamlandi)
			};

			// =====================================================
			// 1️⃣ RANDEVU DURUMUNA GÖRE ÜCRETLER (PIE)
			// =====================================================
			ViewBag.RandevuUcretleriPie = await _context.Randevular
				.Include(r => r.Hizmet)
				.GroupBy(r => r.Durum)
				.Select(g => new
				{
					Durum = g.Key.ToString(),
					ToplamUcret = g.Sum(x => x.Hizmet.Ucret)
				})
				.ToListAsync();

			// =====================================================
			// 2️⃣ RANDEVU DURUM DAĞILIMI (DONUT)
			// =====================================================
			ViewBag.RandevuDurumDonut = await _context.Randevular
				.GroupBy(r => r.Durum)
				.Select(g => new
				{
					Durum = g.Key.ToString(),
					Adet = g.Count()
				})
				.ToListAsync();

			// =====================================================
			// 3️⃣ EN POPÜLER HİZMETLER (BAR)
			// =====================================================
			ViewBag.PopulerHizmetlerBar = await _context.Randevular
				.Include(r => r.Hizmet)
				.GroupBy(r => r.Hizmet.Ad)
				.Select(g => new
				{
					Hizmet = g.Key,
					Adet = g.Count()
				})
				.OrderByDescending(x => x.Adet)
				.Take(5)
				.ToListAsync();

			// =====================================================
			// 4️⃣ SALONLARA GÖRE YOĞUNLUK (BAR)
			// =====================================================
			ViewBag.SalonYogunlukBar = await _context.Randevular
				.Include(r => r.Salon)
				.GroupBy(r => r.Salon.Ad)
				.Select(g => new
				{
					Salon = g.Key,
					Adet = g.Count()
				})
				.OrderByDescending(x => x.Adet)
				.ToListAsync();

			// =====================================================
			// GENEL SİSTEM İSTATİSTİKLERİ
			// =====================================================
			ViewBag.SalonCount = await _context.Salonlar.CountAsync();
			ViewBag.HizmetCount = await _context.Hizmetler.CountAsync();
			ViewBag.KampanyaCount = await _context.Kampanyalar.CountAsync();
			ViewBag.RandevuCount = await _context.Randevular.CountAsync();

			ViewBag.RecentTrainers = await _context.Users
				.OrderByDescending(u => u.Id)
				.Take(5)
				.ToListAsync();

			ViewBag.RecentSalons = await _context.Salonlar
				.OrderByDescending(s => s.Id)
				.Take(5)
				.ToListAsync();

			return View(model);
		}


		// =====================================================
		// TÜM RANDEVULAR
		// =====================================================
		public async Task<IActionResult> TumRandevular()
		{
			var randevular = await _context.Randevular
				.Include(r => r.Antrenor)
				.Include(r => r.Uye)
				.Include(r => r.Hizmet)
				.Include(r => r.Salon)
				.OrderByDescending(r => r.Tarih)
				.ToListAsync();

			return View(randevular.ToRandevularListViewModel("Admin"));
		}

		public async Task<IActionResult> BekleyenRandevular()
		{
			var randevular = await _context.Randevular
				.Where(r => r.Durum == RandevuDurum.Beklemede)
				.Include(r => r.Antrenor)
				.Include(r => r.Uye)
				.Include(r => r.Hizmet)
				.Include(r => r.Salon)
				.OrderBy(r => r.Tarih)
				.ThenBy(r => r.BaslangicSaati)
				.ToListAsync();

			return View(randevular.ToRandevularListViewModel("Admin"));
		}

		public async Task<IActionResult> OnaylananRandevular()
		{
			var randevular = await _context.Randevular
				.Where(r => r.Durum == RandevuDurum.Onaylandi)
				.Include(r => r.Antrenor)
				.Include(r => r.Uye)
				.Include(r => r.Hizmet)
				.Include(r => r.Salon)
				.OrderBy(r => r.Tarih)
				.ToListAsync();

			return View(randevular.ToRandevularListViewModel("Admin"));
		}

		public async Task<IActionResult> TamamlananRandevular()
		{
			var randevular = await _context.Randevular
				.Where(r => r.Durum == RandevuDurum.Tamamlandi)
				.Include(r => r.Antrenor)
				.Include(r => r.Uye)
				.Include(r => r.Hizmet)
				.Include(r => r.Salon)
				.OrderByDescending(r => r.Tarih)
				.ToListAsync();

			return View(randevular.ToRandevularListViewModel("Admin"));
		}

		public async Task<IActionResult> ReddedilenRandevular()
		{
			var randevular = await _context.Randevular
				.Where(r => r.Durum == RandevuDurum.Reddedildi)
				.Include(r => r.Antrenor)
				.Include(r => r.Uye)
				.Include(r => r.Hizmet)
				.Include(r => r.Salon)
				.OrderByDescending(r => r.Tarih)
				.ToListAsync();

			return View(randevular.ToRandevularListViewModel("Admin"));
		}

		public async Task<IActionResult> IptalEdilenRandevular()
		{
			var randevular = await _context.Randevular
				.Where(r => r.Durum == RandevuDurum.Iptal)
				.Include(r => r.Antrenor)
				.Include(r => r.Uye)
				.Include(r => r.Hizmet)
				.Include(r => r.Salon)
				.OrderByDescending(r => r.Tarih)
				.ToListAsync();

			return View(randevular.ToRandevularListViewModel("Admin"));
		}
	}
}
