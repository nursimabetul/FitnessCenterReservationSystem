using FitnessCenterReservationSystem.Data;
using FitnessCenterReservationSystem.Models;
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

		public async Task<IActionResult> Index()
		{
			// Antrenör Id (string)
			var antrenorIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

			// Eğer Antrenör Id int olarak tutuluyorsa parse et
			// Bu durumda ApplicationUser Id string olduğu için direkt string kullanabiliriz
			string antrenorId = antrenorIdString;

			// Aktif randevular (tarih >= şimdi)
			var activeAppointments = await _context.Randevular
				.Include(r => r.Uye)
				.Include(r => r.Hizmet)
				.Where(r => r.AntrenorId == antrenorId && r.Tarih >= DateTime.Now)
				.ToListAsync();

			// Toplam hizmetler (AntrenorHizmet tablosu üzerinden)
			var totalServices = await _context.AntrenorHizmetler
				.Where(a => a.AntrenorId == antrenorId)
				.CountAsync();

			// Toplam uzmanlık alanları (AntrenorUzmanlik tablosu üzerinden)
			var totalSpecializations = await _context.AntrenorUzmanlikAlanlari
				.Where(a => a.AntrenorId == antrenorId)
				.CountAsync();

			// Yaklaşan randevular (top 5)
			var upcomingAppointments = activeAppointments
				.OrderBy(r => r.Tarih)
				.Take(5)
				.ToList();

			ViewBag.ActiveAppointmentsCount = activeAppointments.Count;
			ViewBag.TotalServices = totalServices;
			ViewBag.TotalSpecializations = totalSpecializations;
			ViewBag.UpcomingAppointments = upcomingAppointments;

			return View();
		}
	}
}
