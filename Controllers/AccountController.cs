using FitnessCenterReservationSystem.Models;
using FitnessCenterReservationSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterReservationSystem.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(
			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager,
			SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
		}

		// GET: Register
		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		// POST: Register
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var user = new ApplicationUser
			{
				UserName = model.Email,
				Email = model.Email,
				Ad = model.Ad,
				Soyad = model.Soyad,
				DogumTarihi = model.DogumTarihi,
				Boy = model.Boy,
				Kilo = model.Kilo,
				EmailConfirmed = true,
				SecurityStamp = Guid.NewGuid().ToString()
			};

			var result = await _userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
			{
				// Role ekleme
				if (!string.IsNullOrEmpty(model.Role))
				{
					// Rol yoksa oluştur
					if (!await _roleManager.RoleExistsAsync(model.Role))
						await _roleManager.CreateAsync(new IdentityRole(model.Role));

					await _userManager.AddToRoleAsync(user, model.Role);
				}

				// Oturum aç
				await _signInManager.SignInAsync(user, isPersistent: false);

				TempData["Success"] = "Kayıt işlemi başarılı!";

				return RedirectToAction("Index", "Home");
			}

			// Hataları ekle
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}

			return View(model);
		}
	}
}
