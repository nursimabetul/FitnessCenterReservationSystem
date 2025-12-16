using FitnessCenterReservationSystem.Data;
using FitnessCenterReservationSystem.Models;
using FitnessCenterReservationSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace FitnessCenterReservationSystem.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		private readonly ApplicationDbContext _context;

		public AccountController(
			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager,
			SignInManager<ApplicationUser> signInManager,
			ApplicationDbContext context)  // <-- ekledik
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
			_context = context; // <-- ekledik
		}

		// GET: Register
		[HttpGet]
		public IActionResult Register()
		{
			ViewData["Roles"] = new List<string> { "Üye", "Antrenör", "Admin" }; // Dropdown için
			return View();
		}

		// POST: Register
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
			{
				ViewData["Roles"] = new List<string> { "Üye", "Antrenör", "Admin" };
				return View(model);
			}

			var user = new ApplicationUser
			{
				UserName = model.Email,
				Email = model.Email,
				Ad = model.Ad,
				Soyad = model.Soyad,
				DogumTarihi = model.DogumTarihi,
				Boy = model.Boy,
				Kilo = model.Kilo,
				EmailConfirmed = false, // Admin onayı veya email doğrulama için false
				SecurityStamp = Guid.NewGuid().ToString()
			};

			var result = await _userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
			{
				// Rol ekleme
				var roleName = string.IsNullOrEmpty(model.Role) ? "Üye" : model.Role;

				if (!await _roleManager.RoleExistsAsync(roleName))
					await _roleManager.CreateAsync(new IdentityRole(roleName));

				await _userManager.AddToRoleAsync(user, roleName);

				TempData["Success"] = "Kayıt işlemi başarılı! Admin onayı bekleniyor.";

				return RedirectToAction("Index", "Home");
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}

			ViewData["Roles"] = new List<string> { "Üye", "Antrenör", "Admin" };
			return View(model);
		}

		// GET: ChangePassword
		[HttpGet]
		public async Task<IActionResult> ChangePassword()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
				return RedirectToAction("Login");

			var model = new ChangePasswordViewModel
			{
				Email = user.Email
			};

			return View(model);
		}

		// POST: ChangePassword
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null)
				return RedirectToAction("Login");

			var token = await _userManager.GeneratePasswordResetTokenAsync(user);
			var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

			if (result.Succeeded)
			{
				TempData["Success"] = "Şifreniz başarıyla değiştirildi.";
				return RedirectToAction("Login");
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}

			return View(model);
		}


		// GET: Login
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		// POST: Login
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null)
			{
				ModelState.AddModelError("", "Geçersiz email veya şifre.");
				return View(model);
			}

			// Email doğrulama kontrolü
			if (!user.EmailConfirmed)
			{
				ModelState.AddModelError("", "Hesabınız henüz onaylanmamış. Lütfen email doğrulamasını yapın veya admin onayını bekleyin.");
				return View(model);
			}

			// Giriş işlemi
			var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);


			if (!result.Succeeded)
			{
				ModelState.AddModelError("", "Geçersiz email veya şifre.");
				return View(model);
			}

			// ROLE GÖRE YÖNLENDİRME
			var roles = await _userManager.GetRolesAsync(user);

			if (roles.Contains("Admin"))
				return RedirectToAction("Index", "Admin");

			if (roles.Contains("Antrenör"))
				return RedirectToAction("Index", "Antrenor");

			// Varsayılan → Üye
			return RedirectToAction("Index", "Uye");
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction ("Index", "Home");
			
		}


		[HttpGet]
		public IActionResult AccessDenied()
		{
			return View();
		}


		[HttpGet]
		public async Task<IActionResult> Profile()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
				return RedirectToAction("Login");

			var roles = await _userManager.GetRolesAsync(user);
			bool isAntrenor = roles.Contains("Antrenör");

			var model = new ProfileViewModel
			{
				Id = user.Id,
				Ad = user.Ad,
				Soyad = user.Soyad,
				Email = user.Email,
				DogumTarihi = user.DogumTarihi,
				Boy = user.Boy,
				Kilo = user.Kilo
			};

			if (isAntrenor)
			{
				// Salon listesi
				model.Salonlar = _context.Salonlar
					.Select(s => new SelectListItem { Text = s.Ad, Value = s.Id.ToString() })
					.ToList();

				model.SalonId = user.SalonId;

				// Uzmanlık alanları
				model.UzmanlikAlanlari = _context.UzmanlikAlanlari
					.Select(u => new SelectListItem { Text = u.Ad, Value = u.Id.ToString() })
					.ToList();

				model.SecilenUzmanliklar = _context.AntrenorUzmanlikAlanlari
					.Where(a => a.AntrenorId == user.Id)
					.Select(a => a.UzmanlikAlaniId)
					.ToList();
			}

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Profile(ProfileViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var user = await _userManager.FindByIdAsync(model.Id);
			if (user == null)
				return RedirectToAction("Login");

			user.Ad = model.Ad;
			user.Soyad = model.Soyad;
			user.DogumTarihi = model.DogumTarihi;
			user.Boy = model.Boy;
			user.Kilo = model.Kilo;

			// Antrenör alanları
			var roles = await _userManager.GetRolesAsync(user);
			if (roles.Contains("Antrenör"))
			{
				user.SalonId = model.SalonId;

				// Uzmanlıkları güncelle
				var mevcut = _context.AntrenorUzmanlikAlanlari.Where(a => a.AntrenorId == user.Id);
				_context.AntrenorUzmanlikAlanlari.RemoveRange(mevcut);

				if (model.SecilenUzmanliklar != null)
				{
					foreach (var uzmId in model.SecilenUzmanliklar)
					{
						_context.AntrenorUzmanlikAlanlari.Add(new AntrenorUzmanlik
						{
							AntrenorId = user.Id,
							UzmanlikAlaniId = uzmId
						});
					}
				}
			}

			await _userManager.UpdateAsync(user);
			await _context.SaveChangesAsync();

			TempData["Success"] = "Profiliniz güncellendi.";
			return RedirectToAction("Profile");
		}

	}
}
