using FitnessCenterReservationSystem.Data;
using FitnessCenterReservationSystem.Models;
using FitnessCenterReservationSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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

		// GET: Randevu/Index

		[Authorize(Roles = "Admin,Üye,Antrenör")]
		public async Task<IActionResult> Index(string tarihFilter = null)
		{
			IQueryable<Randevu> randevular = _context.Randevular
				.Include(r => r.Uye)
				.Include(r => r.Antrenor)
				.Include(r => r.Hizmet)
				.Include(r => r.Salon);

			if (User.IsInRole("Üye"))
			{
				var uyeId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
				randevular = randevular.Where(r => r.UyeId == uyeId);
			}
			else if (User.IsInRole("Antrenör"))
			{
				var antrenorId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
				randevular = randevular.Where(r => r.AntrenorId == antrenorId);

				// Tarih filtresi varsa uygula
				if (!string.IsNullOrEmpty(tarihFilter) && DateTime.TryParse(tarihFilter, out var filtreTarih))
				{
					randevular = randevular.Where(r => r.Tarih == filtreTarih);
				}
			}

			return View(await randevular.OrderBy(r => r.Tarih).ToListAsync());
		}



		// GET: Randevu/Create
		[Authorize(Roles = "Admin,Üye")]
		public async Task<IActionResult> Create()
		{
			var model = new RandevuViewModel
			{
				Uyeler = await _context.Users
					.Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == "Üye"))
					.Select(u => new SelectListItem { Value = u.Id, Text = u.UserName })
					.ToListAsync(),
				Antrenorler = await _context.Users
					.Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == "Antrenör"))
					.Select(u => new SelectListItem { Value = u.Id, Text = u.UserName })
					.ToListAsync(),
				Hizmetler = await _context.Hizmetler
					.Select(h => new SelectListItem { Value = h.Id.ToString(), Text = h.Ad })
					.ToListAsync(),
				Salonlar = await _context.Salonlar
					.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Ad })
					.ToListAsync(),
				Durum = RandevuDurum.Beklemede
			};

			return View(model);
		}

		// POST: Randevu/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,Üye")]
		public async Task<IActionResult> Create(RandevuViewModel model)
		{
			if (!ModelState.IsValid) return View(model);

			// Çakışma kontrolü
			bool conflict = await _context.Randevular.AnyAsync(r =>
				r.AntrenorId == model.AntrenorId &&
				r.Tarih == model.Tarih &&
				((r.BaslangicSaati <= model.BaslangicSaati && r.BitisSaati > model.BaslangicSaati) ||
				 (r.BaslangicSaati < model.BitisSaati && r.BitisSaati >= model.BitisSaati))
			);

			if (conflict)
			{
				TempData["Error"] = "Seçilen saatte antrenör başka bir randevuda.";
				return View(model);
			}

			var randevu = new Randevu
			{
				UyeId = model.UyeId,
				AntrenorId = model.AntrenorId,
				HizmetId = model.HizmetId,
				SalonId = model.SalonId,
				Tarih = model.Tarih,
				BaslangicSaati = model.BaslangicSaati,
				BitisSaati = model.BitisSaati,
				Durum = model.Durum
			};

			_context.Randevular.Add(randevu);
			await _context.SaveChangesAsync();

			TempData["Success"] = "Randevu başarıyla oluşturuldu.";
			return RedirectToAction(nameof(Index));
		}

		// GET: Randevu/Edit/5
		[Authorize(Roles = "Admin,Üye")]
		public async Task<IActionResult> Edit(int id)
		{
			var randevu = await _context.Randevular.FindAsync(id);
			if (randevu == null) return NotFound();

			var model = new RandevuViewModel
			{
				Id = randevu.Id,
				UyeId = randevu.UyeId,
				AntrenorId = randevu.AntrenorId,
				HizmetId = randevu.HizmetId,
				SalonId = randevu.SalonId,
				Tarih = randevu.Tarih,
				BaslangicSaati = randevu.BaslangicSaati,
				BitisSaati = randevu.BitisSaati,
				Durum = (RandevuDurum)randevu.Durum,
				Uyeler = await _context.Users
					.Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == "Üye"))
					.Select(u => new SelectListItem { Value = u.Id, Text = u.UserName })
					.ToListAsync(),
				Antrenorler = await _context.Users
					.Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == "Antrenör"))
					.Select(u => new SelectListItem { Value = u.Id, Text = u.UserName })
					.ToListAsync(),
				Hizmetler = await _context.Hizmetler.Select(h => new SelectListItem { Value = h.Id.ToString(), Text = h.Ad }).ToListAsync(),
				Salonlar = await _context.Salonlar.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Ad }).ToListAsync()
			};

			return View(model);
		}

		// POST: Randevu/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,Üye")]
		public async Task<IActionResult> Edit(int id, RandevuViewModel model)
		{
			if (!ModelState.IsValid) return View(model);
			if (id != model.Id) return NotFound();

			bool conflict = await _context.Randevular.AnyAsync(r =>
				r.Id != model.Id &&
				r.AntrenorId == model.AntrenorId &&
				r.Tarih == model.Tarih &&
				((r.BaslangicSaati <= model.BaslangicSaati && r.BitisSaati > model.BaslangicSaati) ||
				 (r.BaslangicSaati < model.BitisSaati && r.BitisSaati >= model.BitisSaati))
			);

			if (conflict)
			{
				TempData["Error"] = "Seçilen saatte antrenör başka bir randevuda.";
				return View(model);
			}

			var randevu = await _context.Randevular.FindAsync(id);
			randevu.UyeId = model.UyeId;
			randevu.AntrenorId = model.AntrenorId;
			randevu.HizmetId = model.HizmetId;
			randevu.SalonId = model.SalonId;
			randevu.Tarih = model.Tarih;
			randevu.BaslangicSaati = model.BaslangicSaati;
			randevu.BitisSaati = model.BitisSaati;
			randevu.Durum = model.Durum;

			_context.Update(randevu);
			await _context.SaveChangesAsync();

			TempData["Success"] = "Randevu başarıyla güncellendi.";
			return RedirectToAction(nameof(Index));
		}

		// GET: Randevu/Delete/5
		[Authorize(Roles = "Admin,Üye")]
		public async Task<IActionResult> Delete(int id)
		{
			var randevu = await _context.Randevular
				.Include(r => r.Uye)
				.Include(r => r.Antrenor)
				.Include(r => r.Hizmet)
				.Include(r => r.Salon)
				.FirstOrDefaultAsync(r => r.Id == id);

			if (randevu == null) return NotFound();

			return View(randevu);
		}

		// POST: Randevu/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,Üye")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var randevu = await _context.Randevular.FindAsync(id);
			if (randevu != null)
			{
				_context.Randevular.Remove(randevu);
				await _context.SaveChangesAsync();
				TempData["Success"] = "Randevu başarıyla silindi.";
			}

			return RedirectToAction(nameof(Index));
		}

		// ================= API ENDPOINTLER =================

		[HttpGet("api/uye/{uyeId}/randevular")]
		[Authorize(Roles = "Admin,Üye")]
		public async Task<IActionResult> UyeRandevulari(string uyeId)
		{
			var list = await _context.Randevular
				.Include(r => r.Hizmet)
				.Include(r => r.Antrenor)
				.Include(r => r.Salon)
				.Where(r => r.UyeId == uyeId)
				.ToListAsync();

			return Ok(list);
		}

		[HttpGet("api/antrenor/{antrenorId}/randevular")]
		[Authorize(Roles = "Admin,Antrenör")]
		public async Task<IActionResult> AntrenorRandevulari(string antrenorId)
		{
			var list = await _context.Randevular
				.Include(r => r.Hizmet)
				.Include(r => r.Uye)
				.Include(r => r.Salon)
				.Where(r => r.AntrenorId == antrenorId)
				.ToListAsync();

			return Ok(list);
		}

		[HttpGet("api/musait-antrenorler")]
		[Authorize(Roles = "Admin,Üye")]
		public async Task<IActionResult> MusaitAntrenorler(DateTime date, TimeSpan startTime, TimeSpan endTime)
		{
			var allAntrenorler = await _context.Users
				.Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == "Antrenör"))
				.ToListAsync();

			var musait = allAntrenorler.Where(a =>
				!_context.Randevular.Any(r =>
					r.AntrenorId == a.Id &&
					r.Tarih == date &&
					((r.BaslangicSaati <= startTime && r.BitisSaati > startTime) ||
					 (r.BaslangicSaati < endTime && r.BitisSaati >= endTime))
				)
			).ToList();

			return Ok(musait);
		}
	}
}
