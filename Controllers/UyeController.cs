using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterReservationSystem.Controllers
{
	public class UyeController : Controller
	{
		[Authorize(Roles = "Üye")]
		public IActionResult Index()
		{
			return View();
		}
	}
}
