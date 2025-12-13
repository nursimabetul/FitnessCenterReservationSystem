using System.ComponentModel.DataAnnotations;

namespace FitnessCenterReservationSystem.ViewModels
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "Adınızı giriniz.")]
		[Display(Name = "Ad")]
		public string Ad { get; set; }

		[Required(ErrorMessage = "Soyadınızı giriniz.")]
		[Display(Name = "Soyad")]
		public string Soyad { get; set; }

		[Required(ErrorMessage = "Email adresinizi giriniz.")]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Şifrenizi giriniz.")]
		[DataType(DataType.Password)]
		[Display(Name = "Şifre")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Şifrenizi tekrar giriniz.")]
		[DataType(DataType.Password)]
		[Display(Name = "Şifreyi Onayla")]
		[Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
		public string ConfirmPassword { get; set; }

		[Display(Name = "Doğum Tarihi")]
		[DataType(DataType.Date)]
		public DateTime? DogumTarihi { get; set; }

		[Display(Name = "Boy (cm)")]
		public double? Boy { get; set; }

		[Display(Name = "Kilo (kg)")]
		public double? Kilo { get; set; }

		[Display(Name = "Kullanıcı Tipi")]
		[Required(ErrorMessage = "Kullanıcı tipini seçiniz.")]
		public string Role { get; set; } // Admin, Üye veya Antrenör
	}
}
