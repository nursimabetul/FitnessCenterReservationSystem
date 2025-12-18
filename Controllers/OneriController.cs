using FitnessCenterReservationSystem.Services;
using FitnessCenterReservationSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterReservationSystem.Controllers
{
	public class OneriController : Controller
	{
		private readonly YapayZekaServisi _yapayZeka;

		public OneriController(YapayZekaServisi yapayZeka)
		{
			_yapayZeka = yapayZeka;
		}

		// GET: Formu göster
		[HttpGet]
		public IActionResult Index()
		{
			return View(new KullaniciBilgiViewModel());
		}

		// POST: Formu gönder ve AI önerisini al
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Index(KullaniciBilgiViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			try
			{
				// AI servisinden öneri al
				var sonuc = await _yapayZeka.EgzersizVeBeslenmeOnerisi(model);
				ViewBag.Oneri = sonuc;
			}
			catch (Exception ex)
			{
				ViewBag.Hata = "Öneri alınırken bir hata oluştu: " + ex.Message;
			}

			return View(model);
		}
	}
}
