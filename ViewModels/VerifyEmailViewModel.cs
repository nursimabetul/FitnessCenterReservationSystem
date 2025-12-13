using System.ComponentModel.DataAnnotations;

namespace FitnessCenterReservationSystem.ViewModels
{
	public class VerifyEmailViewModel
	{
		[Required(ErrorMessage = "Email adresi gereklidir.")]
		[EmailAddress(ErrorMessage = "Geçerli bir email giriniz.")]
		[Display(Name = "Email")]
		public string Email { get; set; }
	}
}
