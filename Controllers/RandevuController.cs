using FitnessCenterReservationSystem.Data;
using FitnessCenterReservationSystem.Models;
using FitnessCenterReservationSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterReservationSystem.Controllers
{
	[Authorize]
	public class RandevuController : Controller
	{
		private readonly ApplicationDbContext _context;

		public RandevuController(ApplicationDbContext context)
		{
			_context = context;
		}


		[HttpGet]
		public async Task<JsonResult> GetHizmetlerBySalon(int id)
		{
			var hizmetler = await _context.Hizmetler
				.Where(h => h.SalonId == id)
				.Select(h => new { value = h.Id, text = h.Ad, sure = h.SureDakika})
				.ToListAsync();

			return Json(hizmetler);
		}
		[HttpGet]
		public async Task<JsonResult> GetAntrenorlerByHizmet(int id)
		{
			var antrenorler = await _context.AntrenorHizmetler
				.Where(ah => ah.HizmetId == id)
				.Select(ah => new { value = ah.AntrenorId, text = ah.Antrenor.Ad + " " + ah.Antrenor.Soyad })
				.ToListAsync();

			return Json(antrenorler);
		}


		[HttpGet]
		public async Task<JsonResult> GetMusaitSaatler(string antrenorId, string tarih)
		{
			// Tarihi parse et
			if (!DateTime.TryParse(tarih, out DateTime dt))
				return Json(new List<string>());

			// 1️⃣ Antrenörün o gün çalışma saatlerini al
			var calismaSaati = await _context.AntrenorCalismaSaatleri
				.Where(cs => cs.AntrenorId == antrenorId && cs.Gun == dt.DayOfWeek)
				.FirstOrDefaultAsync();

			if (calismaSaati == null)
				return Json(new List<string>()); // Çalışma saati yoksa boş dön

			// 2️⃣ Antrenörün o günki mevcut randevularını al
			var randevular = await _context.Randevular
				.Where(r => r.AntrenorId == antrenorId &&
							r.Tarih == dt &&
							r.Durum != RandevuDurum.Iptal)
				.ToListAsync();

			// 3️⃣ Saat aralıklarını 30 dk bloklarla oluştur
			var saatler = new List<string>();
			var baslangic = calismaSaati.BaslangicSaati;
			while (baslangic < calismaSaati.BitisSaati)
			{
				var bitis = baslangic.Add(TimeSpan.FromMinutes(30));

				// Çakışma kontrolü
				bool cakisma = randevular.Any(r =>
					r.BaslangicSaati < bitis && baslangic < r.BitisSaati);

				if (!cakisma)
					saatler.Add($"{baslangic:hh\\:mm}"); // Sadece başlangıç saati

				baslangic = baslangic.Add(TimeSpan.FromMinutes(30));
			}

			return Json(saatler);
		}



		// =====================================================
		// ÜYE
		// =====================================================

		[Authorize(Roles = "Üye")]
		public async Task<IActionResult> Create()
		{
			var model = new RandevuViewModel
			{
				Antrenorler = await _context.Users
					.Where(u => u.AntrenorHizmetler.Any())
					.Select(u => new SelectListItem { Value = u.Id, Text = u.Ad + " " + u.Soyad })
					.ToListAsync(),

				Hizmetler = await _context.Hizmetler
					.Select(h => new SelectListItem { Value = h.Id.ToString(), Text = h.Ad })
					.ToListAsync(),

				Salonlar = await _context.Salonlar
					.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Ad })
					.ToListAsync()
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]

		[HttpPost]
		[Authorize(Roles = "Üye")]
		public async Task<IActionResult> Create(RandevuViewModel model)
		{
			// Dropdownları her zaman doldur
			model.Salonlar = await _context.Salonlar
				.Select(s => new SelectListItem
				{
					Value = s.Id.ToString(),
					Text = s.Ad,
					Selected = s.Id == model.SalonId
				})
				.ToListAsync();

			model.Hizmetler = model.SalonId > 0
				? await _context.Hizmetler
					.Where(h => h.SalonId == model.SalonId)
					.Select(h => new SelectListItem
					{
						Value = h.Id.ToString(),
						Text = h.Ad,
						Selected = h.Id == model.HizmetId
					})
					.ToListAsync()
				: new List<SelectListItem>();

			model.Antrenorler = model.HizmetId > 0
				? await _context.AntrenorHizmetler
					.Where(ah => ah.HizmetId == model.HizmetId)
					.Select(ah => new SelectListItem
					{
						Value = ah.AntrenorId,
						Text = ah.Antrenor.Ad + " " + ah.Antrenor.Soyad,
						Selected = ah.AntrenorId == model.AntrenorId
					})
					.ToListAsync()
				: new List<SelectListItem>();

			var errors = new List<string>();

			// Validasyonlar
			if (model.Tarih.Date < DateTime.Today)
				errors.Add("Geçmiş tarih için randevu alınamaz.");

			if (!await _context.Hizmetler.AnyAsync(h => h.Id == model.HizmetId && h.SalonId == model.SalonId))
				errors.Add("Seçilen salon bu hizmeti sunmamaktadır.");

			if (!await _context.AntrenorHizmetler.AnyAsync(x => x.AntrenorId == model.AntrenorId && x.HizmetId == model.HizmetId))
				errors.Add("Antrenör bu hizmeti verememektedir.");

			var salon = await _context.Salonlar.FindAsync(model.SalonId);
			if (salon == null)
				errors.Add("Seçilen salon bulunamadı.");
			else if (model.BaslangicSaati < salon.AcilisSaati || model.BitisSaati > salon.KapanisSaati)
				errors.Add($"Salon {salon.AcilisSaati:hh\\:mm} - {salon.KapanisSaati:hh\\:mm} saatleri arasında hizmet vermektedir.");

			if (!await _context.AntrenorCalismaSaatleri.AnyAsync(cs =>
				cs.AntrenorId == model.AntrenorId &&
				cs.Gun == model.Tarih.DayOfWeek &&
				model.BaslangicSaati >= cs.BaslangicSaati &&
				model.BitisSaati <= cs.BitisSaati))
				errors.Add("Antrenör bu saatlerde çalışmamaktadır.");

			if (await _context.Randevular.AnyAsync(r =>
				r.AntrenorId == model.AntrenorId &&
				r.Tarih == model.Tarih &&
				r.Durum != RandevuDurum.Iptal &&
				(r.BaslangicSaati < model.BitisSaati && model.BaslangicSaati < r.BitisSaati)))
				errors.Add("Bu saat aralığında antrenörün başka bir randevusu bulunmaktadır.");

			if (errors.Any())
				return Json(new { success = false, errors });

			var uyeId = await _context.Users
				.Where(u => u.UserName == User.Identity!.Name)
				.Select(u => u.Id)
				.FirstAsync();

			var randevu = new Randevu
			{
				UyeId = uyeId,
				AntrenorId = model.AntrenorId,
				SalonId = model.SalonId,
				HizmetId = model.HizmetId,
				Tarih = model.Tarih,
				BaslangicSaati = model.BaslangicSaati,
				BitisSaati = model.BitisSaati,
				Durum = RandevuDurum.Beklemede
			};

			_context.Randevular.Add(randevu);
			await _context.SaveChangesAsync();

			return Json(new { success = true, message = "Randevu talebiniz oluşturuldu." });
		}



		[Authorize(Roles = "Admin,Antrenör")]
		// Ortak Onayla
		[HttpPost]
		public async Task<IActionResult> Onayla(int id, string returnUrl = null)
		{
			var r = await _context.Randevular.FindAsync(id);
			if (r == null) return NotFound();

			r.Durum = RandevuDurum.Onaylandi;
			await _context.SaveChangesAsync();

			return RedirectToLocal(returnUrl);
		}

		//  Ortak Reddet
		[HttpPost]
		[Authorize(Roles = "Admin,Antrenör")]
		public async Task<IActionResult> Reddet(int id, string returnUrl = null)
		{
			var r = await _context.Randevular.FindAsync(id);
			if (r == null) return NotFound();

			r.Durum = RandevuDurum.Reddedildi;
			await _context.SaveChangesAsync();

			return RedirectToLocal(returnUrl);
		}

		// Ortak İptal
		[HttpPost]
		[Authorize(Roles = "Admin,Antrenör,Üye")]
		public async Task<IActionResult> Iptal(int id, string returnUrl = null)
		{
			var r = await _context.Randevular.FindAsync(id);
			if (r == null) return NotFound();

			r.Durum = RandevuDurum.Iptal;
			await _context.SaveChangesAsync();

			return RedirectToLocal(returnUrl);
		}

		//Ortak Tamamla
		[HttpPost]
		[Authorize(Roles = "Admin,Antrenör,Üye")]
		public async Task<IActionResult> Tamamla(int id, string returnUrl = null)
		{
			var r = await _context.Randevular.FindAsync(id);
			if (r == null) return NotFound();

			r.Durum = RandevuDurum.Tamamlandi;
			await _context.SaveChangesAsync();

			return RedirectToLocal(returnUrl);
		}

		// Ortak redirect
		private IActionResult RedirectToLocal(string returnUrl)
		{
			if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
				return Redirect(returnUrl);

			// default olarak Randevularim sayfasına dön
			return RedirectToAction("Randevularim");
		}




	}
}
