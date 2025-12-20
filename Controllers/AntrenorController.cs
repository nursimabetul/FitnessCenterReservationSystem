using FitnessCenterReservationSystem.Data;
using FitnessCenterReservationSystem.Extensions;
using FitnessCenterReservationSystem.Models;
using FitnessCenterReservationSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FitnessCenterReservationSystem.Controllers
{
	[Authorize(Roles = "Antrenör")]
	public class AntrenorController : Controller
	{
		private readonly ApplicationDbContext _context;

		public AntrenorController(ApplicationDbContext context)
		{
			_context = context;
		}


		// =====================================================
		// DASHBOARD
		// =====================================================

		public async Task<IActionResult> Index()
		{
			var antrenorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var today = DateTime.Today;

			// Tamamlanacak randevular
			var tamamlanacaklar = await _context.Randevular
				.Where(r => r.AntrenorId == antrenorId && r.Durum == RandevuDurum.Onaylandi && r.Tarih < today)
				.ToListAsync();

			foreach (var r in tamamlanacaklar)
				r.Durum = RandevuDurum.Tamamlandi;

			await _context.SaveChangesAsync();

			// Aktif randevular (bugünden itibaren)
			var aktifRandevular = await _context.Randevular
				.Where(r => r.AntrenorId == antrenorId
							&& r.Tarih >= today
							&& r.Durum != RandevuDurum.Iptal
							&& r.Durum != RandevuDurum.Reddedildi)
				.Include(r => r.Uye)
				.Include(r => r.Hizmet)
				.Include(r => r.Salon)
				.ToListAsync();


			// Yaklaşan 5 randevu
			var YaklasanRandevular = aktifRandevular
				.OrderBy(r => r.Tarih)
				.Take(5)
				.ToList();

			// Bugünkü randevular
			var bugunRandevular = await _context.Randevular
				.Where(r => r.AntrenorId == antrenorId && r.Tarih == today && r.Durum == RandevuDurum.Onaylandi)
				.Include(r => r.Uye)
				.Include(r => r.Hizmet)
				.Include(r => r.Salon)
				.ToListAsync();
			var role = User.IsInRole("Admin") ? "Admin" : User.IsInRole("Antrenör") ? "Antrenör" : "Üye";           // Modeli oluştur
			var model = new DashboardViewModel
			{

				AktifRandevular = aktifRandevular.Count,
				BugunRandevular = bugunRandevular.ToRandevularListViewModel(role),
				YaklasanRandevular = YaklasanRandevular.ToRandevularListViewModel(role),
				Bekleyen = await _context.Randevular.CountAsync(r => r.AntrenorId == antrenorId && r.Durum == RandevuDurum.Beklemede),
				Onaylanan = await _context.Randevular.CountAsync(r => r.AntrenorId == antrenorId && r.Durum == RandevuDurum.Onaylandi),
				Reddedilen = await _context.Randevular.CountAsync(r => r.AntrenorId == antrenorId && r.Durum == RandevuDurum.Reddedildi),
				IptalEdilen = await _context.Randevular.CountAsync(r => r.AntrenorId == antrenorId && r.Durum == RandevuDurum.Iptal),
				Tamamlanan = await _context.Randevular.CountAsync(r => r.AntrenorId == antrenorId && r.Durum == RandevuDurum.Tamamlandi)
			};

			return View(model);
		}

		// =====================================================
		// RANDEVU LİSTELERİ
		// =====================================================
		// =====================================================
		// Tüm Randevular
		// =====================================================
		public async Task<IActionResult> TumRandevular()
		{
			var antrenorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var randevular = await _context.Randevular
				.Where(r => r.AntrenorId == antrenorId)
				.Include(r => r.Antrenor) 
				.Include(r => r.Uye)
				.Include(r => r.Hizmet)
				.Include(r => r.Salon)
				.OrderByDescending(r => r.Tarih)
				.ToListAsync();
			var role = User.IsInRole("Admin") ? "Admin" : User.IsInRole("Antrenör") ? "Antrenör" : "Üye";
			var model = randevular.ToRandevularListViewModel(role);
			return View(model);
		}

		// =====================================================
		// Bekleyen Randevular
		// =====================================================
		public async Task<IActionResult> BekleyenRandevular()
		{
			var antrenorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var randevular = await _context.Randevular
				.Where(r => r.AntrenorId == antrenorId && r.Durum == RandevuDurum.Beklemede)
				.Include(r => r.Antrenor)
				.Include(r => r.Uye)
				.Include(r => r.Hizmet)
				.Include(r => r.Salon)
				.OrderBy(r => r.Tarih)
				.ThenBy(r => r.BaslangicSaati)
				.ToListAsync();
			var role = User.IsInRole("Admin") ? "Admin" : User.IsInRole("Antrenör") ? "Antrenör" : "Üye";
			var model = randevular.ToRandevularListViewModel(role);
			return View(model);
		}

		// =====================================================
		// Onaylanan Randevular
		// =====================================================
		public async Task<IActionResult> OnaylananRandevular()
		{
			var antrenorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var randevular = await _context.Randevular
				.Where(r => r.AntrenorId == antrenorId && r.Durum == RandevuDurum.Onaylandi)
				.Include(r => r.Antrenor)
				.Include(r => r.Uye)
				.Include(r => r.Hizmet)
				.Include(r => r.Salon)
				.OrderBy(r => r.Tarih)
				.ToListAsync();
			var role = User.IsInRole("Admin") ? "Admin" : User.IsInRole("Antrenör") ? "Antrenör" : "Üye";
			var model = randevular.ToRandevularListViewModel(role);
			return View(model);
		}

		// =====================================================
		// Tamamlanan Randevular
		// =====================================================
		public async Task<IActionResult> TamamlananRandevular()
		{
			var antrenorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var randevular = await _context.Randevular
				.Where(r => r.AntrenorId == antrenorId && r.Durum == RandevuDurum.Tamamlandi)
				.Include(r => r.Antrenor)
				.Include(r => r.Uye)
				.Include(r => r.Hizmet)
				.Include(r => r.Salon)
				.OrderByDescending(r => r.Tarih)
				.ToListAsync();
			var role = User.IsInRole("Admin") ? "Admin" : User.IsInRole("Antrenör") ? "Antrenör" : "Üye";
			var model = randevular.ToRandevularListViewModel(role);
			return View(model);
		}

		// =====================================================
		// Reddedilen Randevular
		// =====================================================
		public async Task<IActionResult> ReddedilenRandevular()
		{
			var antrenorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var randevular = await _context.Randevular
				.Where(r => r.AntrenorId == antrenorId && r.Durum == RandevuDurum.Reddedildi)
				.Include(r => r.Antrenor)
				.Include(r => r.Uye)
				.Include(r => r.Hizmet)
				.Include(r => r.Salon)
				.OrderByDescending(r => r.Tarih)
				.ToListAsync();
			var role = User.IsInRole("Admin") ? "Admin" : User.IsInRole("Antrenör") ? "Antrenör" : "Üye";
			var model = randevular.ToRandevularListViewModel(role);
			return View(model);
		}

		// =====================================================
		// İptal Edilen Randevular
		// =====================================================
		public async Task<IActionResult> IptalEdilenRandevular()
		{
			var antrenorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var randevular = await _context.Randevular
				.Where(r => r.AntrenorId == antrenorId && r.Durum == RandevuDurum.Iptal)
				.Include(r => r.Antrenor)
				.Include(r => r.Uye)
				.Include(r => r.Hizmet)
				.Include(r => r.Salon)
				.OrderByDescending(r => r.Tarih)
				.ToListAsync();
			var role = User.IsInRole("Admin") ? "Admin" : User.IsInRole("Antrenör") ? "Antrenör" : "Üye";
			var model = randevular.ToRandevularListViewModel(role);
			return View(model);
		}




		// Bekleyen randevular

		public async Task<IActionResult> Randevularim()
		{
			var antrenorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			// Rolü al
			var role = User.IsInRole("Admin") ? "Admin" :
					   User.IsInRole("Antrenör") ? "Antrenör" :
					   User.IsInRole("Üye") ? "Üye" : "";

			var randevular = await _context.Randevular
				.Where(r => r.AntrenorId == antrenorId)
				.Include(r => r.Antrenor)
				.Include(r => r.Uye)
				.Include(r => r.Hizmet)
				.Include(r => r.Salon)
				.OrderByDescending(r => r.Tarih)
				.ThenByDescending(r => r.BaslangicSaati)
				.ToListAsync();

			// Map ederken rolü de gönder
			var model = randevular.Select(r => MapToListViewModel(r, role)).ToList();

			return View(model);
		}

	






		// =====================================================
		// RANDEVU ONAY / RED
		// =====================================================
		public async Task<IActionResult> Approve(int id)
		{
			var antrenorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var randevu = await _context.Randevular
				.FirstOrDefaultAsync(r => r.Id == id && r.AntrenorId == antrenorId && r.Durum == RandevuDurum.Beklemede);

			if (randevu == null) return NotFound();

			randevu.Durum = RandevuDurum.Onaylandi;
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Randevularim));
		}

		public async Task<IActionResult> Reject(int id)
		{
			var antrenorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var randevu = await _context.Randevular
				.FirstOrDefaultAsync(r => r.Id == id && r.AntrenorId == antrenorId && r.Durum == RandevuDurum.Beklemede);

			if (randevu == null) return NotFound();

			randevu.Durum = RandevuDurum.Reddedildi;
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Randevularim));
		}

		// =====================================================
		// HELPERS
		// =====================================================
		private static RandevularListViewModel MapToListViewModel(Randevu r , string userRole)
		{
			return new RandevularListViewModel
			{
				Id = r.Id,
				UyeAdSoyad = r.Uye.Ad + " " + r.Uye.Soyad,
				AnrenörAdSoyad= r.Antrenor.Ad + " " + r.Antrenor.Soyad,
				HizmetAd = r.Hizmet.Ad,
				SalonAd = r.Salon.Ad,
				Tarih = r.Tarih,
				BaslangicSaati = r.BaslangicSaati,
				BitisSaati = r.BitisSaati,
				Durum = r.Durum,
				DurumIcon = GetDurumIcon(r.Durum),
				DurumLabel = r.Durum.ToString(),
				UserRole = userRole


			};
		}

		private static string GetDurumIcon(RandevuDurum durum)
		{
			return durum switch
			{
				RandevuDurum.Onaylandi => "bi-check-circle text-success",
				RandevuDurum.Beklemede => "bi-hourglass-split text-warning",
				RandevuDurum.Iptal => "bi-x-circle text-danger",
				RandevuDurum.Reddedildi => "bi-x-circle text-secondary",
				RandevuDurum.Tamamlandi => "bi-check-lg text-primary",
				_ => "bi-question-circle"
			};
		}
		// =====================================================
		// UZMANLIK ALANLARIM
		// =====================================================
		public async Task<IActionResult> UzmanlikAlanlarim()
		{
			var antrenorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var tumUzmanliklar = await _context.UzmanlikAlanlari.ToListAsync();

			var seciliUzmanliklar = await _context.AntrenorUzmanlikAlanlari
				.Where(x => x.AntrenorId == antrenorId)
				.Select(x => x.UzmanlikAlaniId)
				.ToListAsync();

			var model = tumUzmanliklar.Select(u => new UzmanlikCheckboxVM
			{
				Id = u.Id,
				Ad = u.Ad,
				Secili = seciliUzmanliklar.Contains(u.Id)
			}).ToList();

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UzmanlikAlanlarim(int[] secilenUzmanliklar)
		{
			var antrenorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var mevcutlar = _context.AntrenorUzmanlikAlanlari
				.Where(x => x.AntrenorId == antrenorId);

			_context.AntrenorUzmanlikAlanlari.RemoveRange(mevcutlar);

			foreach (var uzmanlikId in secilenUzmanliklar)
			{
				_context.AntrenorUzmanlikAlanlari.Add(new AntrenorUzmanlik
				{
					AntrenorId = antrenorId,
					UzmanlikAlaniId = uzmanlikId
				});
			}

			await _context.SaveChangesAsync();
			TempData["Success"] = "Uzmanlık alanlarınız başarıyla güncellendi.";

			return RedirectToAction(nameof(UzmanlikAlanlarim));
		}

		// =====================================================
		// HİZMETLERİM
		// =====================================================
		public async Task<IActionResult> Hizmetlerim()
		{
			var antrenorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var tumHizmetler = await _context.Hizmetler
				.Include(h => h.Salon)
				.ToListAsync();

			var seciliHizmetler = await _context.AntrenorHizmetler
				.Where(x => x.AntrenorId == antrenorId)
				.Select(x => x.HizmetId)
				.ToListAsync();

			var model = tumHizmetler.Select(h => new HizmetCheckboxVM
			{
				Id = h.Id,
				Ad = h.Ad,
				SureDakika = h.SureDakika,
				Ucret = h.Ucret,
				SalonAdi = h.Salon.Ad,
				Secili = seciliHizmetler.Contains(h.Id)
			}).ToList();

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Hizmetlerim(int[] secilenHizmetler)
		{
			var antrenorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var mevcutlar = _context.AntrenorHizmetler
				.Where(x => x.AntrenorId == antrenorId);

			_context.AntrenorHizmetler.RemoveRange(mevcutlar);

			foreach (var hizmetId in secilenHizmetler)
			{
				_context.AntrenorHizmetler.Add(new AntrenorHizmet
				{
					AntrenorId = antrenorId,
					HizmetId = hizmetId
				});
			}

			await _context.SaveChangesAsync();
			TempData["Success"] = "Hizmetleriniz başarıyla güncellendi.";

			return RedirectToAction(nameof(Hizmetlerim));
		}

		// =====================================================
		// ÇALIŞMA SAATLERİM
		// =====================================================
		public async Task<IActionResult> CalismaSaatlerim()
		{
			var antrenorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var saatler = await _context.AntrenorCalismaSaatleri
				.Where(x => x.AntrenorId == antrenorId)
				.OrderBy(x => x.Gun)
				.ThenBy(x => x.BaslangicSaati)
				.ToListAsync();

			return View(saatler);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CalismaSaatlerim(List<AntrenorCalismaSaati> model)
		{
			var antrenorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var mevcutlar = _context.AntrenorCalismaSaatleri
				.Where(x => x.AntrenorId == antrenorId);

			_context.AntrenorCalismaSaatleri.RemoveRange(mevcutlar);

			foreach (var item in model)
			{
				item.AntrenorId = antrenorId;
				_context.AntrenorCalismaSaatleri.Add(item);
			}

			await _context.SaveChangesAsync();
			TempData["Success"] = "Çalışma saatleriniz başarıyla güncellendi.";

			return RedirectToAction(nameof(CalismaSaatlerim));
		}










	}
}
