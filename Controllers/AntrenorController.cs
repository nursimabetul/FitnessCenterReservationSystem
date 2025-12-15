using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterReservationSystem.Controllers
{
	[Authorize(Roles = "Antrenör")]
	public class AntrenorController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
