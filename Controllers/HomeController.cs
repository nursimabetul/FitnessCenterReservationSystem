using FitnessCenterReservationSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FitnessCenterReservationSystem.Controllers
{

	public class HomeController : Controller
    {
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<ApplicationUser> _userManager;

		public HomeController(
			ILogger<HomeController> logger,
			UserManager<ApplicationUser> userManager)
		{
			_logger = logger;
			_userManager = userManager;
		}


		public async Task<IActionResult> Index()
		{
			if (User.Identity != null && User.Identity.IsAuthenticated)
			{
				var user = await _userManager.GetUserAsync(User);
				var roles = await _userManager.GetRolesAsync(user);

				if (roles.Contains("Admin"))
					return RedirectToAction("Index", "Admin");

				if (roles.Contains("Antrenör"))
					return RedirectToAction("Index", "Antrenor");

				return RedirectToAction("Index", "Uye");
			}

			return View();
		}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

		public IActionResult Contact()
		{
			return View();
		}
		public IActionResult About()
		{
			return View();
		}
	}
}
