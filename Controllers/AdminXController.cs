
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitnessCenterReservationSystem.Models;

[Authorize(Roles = "Admin")]
public class AdminXController : Controller
{
	private readonly UserManager<ApplicationUser> _userManager;

	public AdminXController(UserManager<ApplicationUser> userManager)
	{
		_userManager = userManager;
	}

	// ⚠️ SADECE DEVELOPMENT ORTAMI
	public async Task<IActionResult> ResetAllPasswords()
	{
		var users = _userManager.Users.ToList();

		foreach (var user in users)
		{
			var token = await _userManager.GeneratePasswordResetTokenAsync(user);
			await _userManager.ResetPasswordAsync(user, token, "sau");
		}

		return Content("Tüm kullanıcıların şifresi 'sau' olarak güncellendi.");
	}
}
