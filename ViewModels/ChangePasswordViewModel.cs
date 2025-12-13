using System.ComponentModel.DataAnnotations;

namespace FitnessCenterReservationSystem.ViewModels
{
	public class ChangePasswordViewModel
	{
		[Required(ErrorMessage = "Email adresinizi giriniz.")]
		[EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Yeni şifrenizi giriniz.")]
		[StringLength(40, MinimumLength = 8, ErrorMessage = "{0} en az {2}, en fazla {1} karakter olmalıdır.")]
		[DataType(DataType.Password)]
		[Display(Name = "Yeni Şifre")]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "Yeni şifrenizi tekrar giriniz.")]
		[DataType(DataType.Password)]
		[Display(Name = "Yeni Şifreyi Onayla")]
		[Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor.")]
		public string ConfirmNewPassword { get; set; }
	}
}
