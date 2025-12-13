using System.ComponentModel.DataAnnotations;

namespace FitnessCenterReservationSystem.ViewModels
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Email alanı zorunludur.")]
		[EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Şifre alanı zorunludur.")]
		[DataType(DataType.Password)]
		[Display(Name = "Şifre")]
		public string Password { get; set; }

		[Display(Name = "Beni Hatırla")]
		public bool RememberMe { get; set; }
	}
}
