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

		// =====================================================
		// 👤 ÜYE
		// =====================================================

		[Authorize(Roles = "Üye")]
		public async Task<IActionResult> Create()
		{
			var model = new RandevuViewModel
			{
				Antrenorler = await _context.Users
					.Where(u => u.AntrenorHizmetler.Any())
					.Select(u => new SelectListItem
					{
						Value = u.Id,
						Text = u.Ad + " " + u.Soyad
					})
					.ToListAsync(),

				Hizmetler = await _context.Hizmetler
					.Select(h => new SelectListItem
					{
						Value = h.Id.ToString(),
						Text = h.Ad
					})
					.ToListAsync(),

				Salonlar = await _context.Salonlar
					.Select(s => new SelectListItem
					{
						Value = s.Id.ToString(),
						Text = s.Ad
					})
					.ToListAsync()
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Üye")]
		public async Task<IActionResult> Create(RandevuViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var uyeId = await _context.Users
				.Where(u => u.UserName == User.Identity!.Name)
				.Select(u => u.Id)
				.FirstAsync();

			// 1️⃣ Geçmiş tarih kontrolü
			if (model.Tarih.Date < DateTime.Today)
			{
				ModelState.AddModelError("", "Geçmiş tarih için randevu alınamaz.");
				return View(model);
			}

			// 2️⃣ Salon bu hizmeti veriyor mu?
			bool salonHizmetiVarMi = await _context.Hizmetler
				.AnyAsync(h => h.Id == model.HizmetId && h.SalonId == model.SalonId);

			if (!salonHizmetiVarMi)
			{
				ModelState.AddModelError("", "Seçilen salon bu hizmeti sunmamaktadır.");
				return View(model);
			}

			// 3️⃣ Antrenör bu hizmeti veriyor mu?
			bool antrenorHizmetiVarMi = await _context.AntrenorHizmetler
				.AnyAsync(x =>
					x.AntrenorId == model.AntrenorId &&
					x.HizmetId == model.HizmetId);

			if (!antrenorHizmetiVarMi)
			{
				ModelState.AddModelError("", "Antrenör bu hizmeti verememektedir.");
				return View(model);
			}

			// 4️⃣ Salon çalışma saatleri
			var salon = await _context.Salonlar.FindAsync(model.SalonId);

			if (salon == null)
			{
				ModelState.AddModelError("", "Salon bulunamadı.");
				return View(model);
			}

			if (model.BaslangicSaati < salon.AcilisSaati ||
				model.BitisSaati > salon.KapanisSaati)
			{
				ModelState.AddModelError("", "Salon bu saatlerde hizmet vermemektedir.");
				return View(model);
			}


			// 5️⃣ Antrenör çalışma saatleri
			bool antrenorMusaitMi = await _context.AntrenorCalismaSaatleri.AnyAsync(cs =>
				cs.AntrenorId == model.AntrenorId &&
				cs.Gun == model.Tarih.DayOfWeek &&
				model.BaslangicSaati >= cs.BaslangicSaati &&
				model.BitisSaati <= cs.BitisSaati
			);

			if (!antrenorMusaitMi)
			{
				ModelState.AddModelError("", "Antrenör bu saatlerde çalışmamaktadır.");
				return View(model);
			}

			// 6️⃣ Çakışma kontrolü
			bool cakismaVarMi = await _context.Randevular.AnyAsync(r =>
				r.AntrenorId == model.AntrenorId &&
				r.Tarih == model.Tarih &&
				r.Durum != RandevuDurum.Iptal &&
				(
					r.BaslangicSaati < model.BitisSaati &&
					model.BaslangicSaati < r.BitisSaati
				)
			);

			if (cakismaVarMi)
			{
				ModelState.AddModelError("", "Bu saat aralığında başka bir randevu bulunmaktadır.");
				return View(model);
			}

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

			TempData["Success"] = "Randevu talebiniz oluşturuldu.";
			return RedirectToAction(nameof(MyRandevular));
		}

		[Authorize(Roles = "Üye")]
		public async Task<IActionResult> MyRandevular()
		{
			var uyeId = await _context.Users
				.Where(u => u.UserName == User.Identity!.Name)
				.Select(u => u.Id)
				.FirstAsync();

			var list = await _context.Randevular
				.Include(r => r.Antrenor)
				.Include(r => r.Hizmet)
				.Include(r => r.Salon)
				.Where(r => r.UyeId == uyeId)
				.OrderByDescending(r => r.Tarih)
				.ToListAsync();

			return View(list);
		}

		[Authorize(Roles = "Üye")]
		public async Task<IActionResult> Cancel(int id)
		{
			var uyeId = await _context.Users
				.Where(u => u.UserName == User.Identity!.Name)
				.Select(u => u.Id)
				.FirstAsync();

			var randevu = await _context.Randevular.FindAsync(id);

			if (randevu == null || randevu.UyeId != uyeId)
				return NotFound();

			randevu.Durum = RandevuDurum.Iptal;
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(MyRandevular));
		}

		// =====================================================
		// 🧑‍🏫 ANTRENÖR
		// =====================================================

		[Authorize(Roles = "Antrenör")]
		public async Task<IActionResult> PendingRandevular()
		{
			var antrenorId = await _context.Users
				.Where(u => u.UserName == User.Identity!.Name)
				.Select(u => u.Id)
				.FirstAsync();

			var list = await _context.Randevular
				.Include(r => r.Uye)
				.Include(r => r.Hizmet)
				.Where(r =>
					r.AntrenorId == antrenorId &&
					r.Durum == RandevuDurum.Beklemede)
				.ToListAsync();

			return View(list);
		}

		[Authorize(Roles = "Antrenör")]
		public async Task<IActionResult> Approve(int id)
		{
			var randevu = await _context.Randevular.FindAsync(id);
			if (randevu == null) return NotFound();

			randevu.Durum = RandevuDurum.Onaylandi;
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(PendingRandevular));
		}

		[Authorize(Roles = "Antrenör")]
		public async Task<IActionResult> Reject(int id)
		{
			var randevu = await _context.Randevular.FindAsync(id);
			if (randevu == null) return NotFound();

			randevu.Durum = RandevuDurum.Reddedildi;
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(PendingRandevular));
		}
	}
}
