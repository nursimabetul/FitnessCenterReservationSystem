using FitnessCenterReservationSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterReservationSystem.Controllers.Api
{
	[ApiController]
	[Route("api/raporlama")]
	public class RaporlamaApiController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public RaporlamaApiController(ApplicationDbContext context)
		{
			_context = context;
		}

		// =====================================================
		// 1️⃣ TÜM ANTRENÖRLER
		// =====================================================
		[HttpGet("antrenorler")]
		public async Task<IActionResult> GetTumAntrenorler()
		{
			var antrenorler = await _context.Users
				.Where(u => u.AntrenorHizmetler.Any())
				.Select(u => new
				{
					u.Id,
					u.Ad,
					u.Soyad
				})
				.ToListAsync();

			return Ok(antrenorler);
		}

		// =====================================================
		// 2️⃣ BELİRLİ TARİHTE MÜSAİT ANTRENÖRLER
		// =====================================================
		[HttpGet("musait-antrenorler")]
		public async Task<IActionResult> GetMusaitAntrenorler(DateTime tarih, TimeSpan baslangic, TimeSpan bitis)
		{
			var gun = tarih.DayOfWeek;

			// Müsait antrenörleri getir
			var musaitAntrenorler = await _context.Users
				.Where(u =>
					u.CalismaSaatleri.Any(cs =>
						cs.Gun == gun &&
						baslangic >= cs.BaslangicSaati &&
						bitis <= cs.BitisSaati
					) &&
					!_context.Randevular.Any(r =>
						r.AntrenorId == u.Id &&
						r.Tarih.Date == tarih.Date &&
						r.Durum != Models.RandevuDurum.Iptal &&
						(r.BaslangicSaati < bitis && baslangic < r.BitisSaati)
					)
				)
				.Select(u => new
				{
					u.Id,
					u.Ad,
					u.Soyad
				})
				.ToListAsync();

			if (!musaitAntrenorler.Any())
				return NotFound(new { message = "Belirtilen tarihte müsait antrenör bulunamadı." });

			return Ok(musaitAntrenorler);
		}

		// =====================================================
		// 3️⃣ ÜYE RANDEVULARI
		// =====================================================
		[Authorize]
		[HttpGet("uye/{uyeId}/randevular")]
		public async Task<IActionResult> GetUyeRandevulari(string uyeId)
		{
			var randevular = await _context.Randevular
				.Where(r => r.UyeId == uyeId)
				.Include(r => r.Antrenor)
				.Include(r => r.Hizmet)
				.Select(r => new
				{
					r.Id,
					r.Tarih,
					r.BaslangicSaati,
					r.BitisSaati,
					Durum = r.Durum.ToString(),
					Antrenor = r.Antrenor!.Ad + " " + r.Antrenor!.Soyad,
					Hizmet = r.Hizmet!.Ad
				})
				.ToListAsync();

			return Ok(randevular);
		}
	}
}
