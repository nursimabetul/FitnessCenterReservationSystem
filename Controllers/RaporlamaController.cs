using FitnessCenterReservationSystem.Data;
using FitnessCenterReservationSystem.Models;
using FitnessCenterReservationSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FitnessCenterReservationSystem.Controllers
{
	public class RaporlamaController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IHttpClientFactory _httpClientFactory;

		public RaporlamaController(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
		{
			_context = context;
			_httpClientFactory = httpClientFactory;
		}

		// =====================================================
		// 1️⃣ Tüm Antrenörler
		// =====================================================
		public async Task<IActionResult> Antrenorler()
		{
			var client = _httpClientFactory.CreateClient();
			var response = await client.GetAsync("https://localhost:7152/api/raporlama/antrenorler");

			if (!response.IsSuccessStatusCode)
			{
				ViewBag.Error = "API çağrısı başarısız oldu.";
				return View(new List<AntrenorViewModel>());
			}

			var json = await response.Content.ReadAsStringAsync();
			var antrenorler = JsonConvert.DeserializeObject<List<AntrenorViewModel>>(json);

			return View(antrenorler);
		}

		// =====================================================
		// 2️⃣ Belirli tarihte müsait antrenörler
		// =====================================================
		// =====================================================
		// Musait Antrenörler
		// =====================================================
		[HttpGet]
		public IActionResult MusaitAntrenorler()
		{
			// Başlangıçta boş form ve liste göster
			return View(new List<dynamic>());
		}

		[HttpPost]
		public async Task<IActionResult> MusaitAntrenorler(DateTime tarih, TimeSpan baslangic, TimeSpan bitis)
		{
			var client = _httpClientFactory.CreateClient();

			// API URL oluşturma (parametreleri ekliyoruz)
			var url = $"https://localhost:7152/api/raporlama/musait-antrenorler?tarih={tarih:yyyy-MM-dd}&baslangic={baslangic}&bitis={bitis}";

			var response = await client.GetAsync(url);

			if (!response.IsSuccessStatusCode)
			{
				ViewBag.Error = "API çağrısı başarısız oldu.";
				return View(new List<dynamic>());
			}

			var json = await response.Content.ReadAsStringAsync();
			var musaitAntrenorler = JsonConvert.DeserializeObject<List<dynamic>>(json);

			// Tarih ve saatleri ViewBag ile geri gönderelim, form tekrar doldurulsun
			ViewBag.Tarih = tarih.ToString("yyyy-MM-dd");
			ViewBag.Baslangic = baslangic.ToString(@"hh\:mm");
			ViewBag.Bitis = bitis.ToString(@"hh\:mm");

			return View(musaitAntrenorler);
		}

		// =====================================================
		// 3️⃣ Üye randevuları
		// =====================================================

		// GET: UyeRandevulari

		// =====================================================
		// ÜYE RANDEVULARI
		// =====================================================
		public async Task<IActionResult> UyeRandevulari(string? uyeId)
		{
			// 1️⃣ Üyeleri getir (dropdown)
			var uyeRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Üye");
			var uyeler = await _context.Users
				.Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == uyeRole.Id))
				.Select(u => new { u.Id, u.Ad, u.Soyad })
				.ToListAsync();

			ViewBag.Uyeler = uyeler;

			if (string.IsNullOrEmpty(uyeId))
				return View(new List<UyeRandevuViewModel>());

			try
			{
				var client = _httpClientFactory.CreateClient();

				// 🌟 Tarayıcıdaki cookie’yi HttpClient’a ekle
				if (Request.Headers.ContainsKey("Cookie"))
					client.DefaultRequestHeaders.Add("Cookie", Request.Headers["Cookie"].ToString());

				var url = $"https://localhost:7152/api/raporlama/uye/{uyeId}/randevular";
				var response = await client.GetAsync(url);

				if (!response.IsSuccessStatusCode)
				{
					var apiContent = await response.Content.ReadAsStringAsync();
					ViewBag.Error = $"API Hatası: {response.StatusCode} - {apiContent}";
					return View(new List<UyeRandevuViewModel>());
				}

				var json = await response.Content.ReadAsStringAsync();
				var randevular = JsonConvert.DeserializeObject<List<UyeRandevuViewModel>>(json);

				return View(randevular);
			}
			catch (Exception ex)
			{
				ViewBag.Error = $"İşlem sırasında hata oluştu: {ex.Message}";
				return View(new List<UyeRandevuViewModel>());
			}
		}

	}



}
