using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FitnessCenterReservationSystem.Models
{
	public class Hizmet
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Hizmet adı zorunludur.")]
		[StringLength(100, ErrorMessage = "Hizmet adı en fazla 100 karakter olabilir.")]
		public string? Ad { get; set; }

		[Range(1, 600, ErrorMessage = "Süre 1-600 dakika arasında olmalıdır.")]
		public int SureDakika { get; set; }

		[Range(0, 10000, ErrorMessage = "Ücret 0-10.000 arasında olmalıdır.")]
		public decimal Ucret { get; set; }

		// Salon ilişkisi
		[Display(Name = "Salon")]
		public int SalonId { get; set; }

		[ValidateNever]
		public Salon Salon { get; set; } = null!;

		// Bu hizmete ait randevular
		[ValidateNever]
		public ICollection<Randevu>? Randevular { get; set; }

		// Hizmeti verebilen antrenörler (pivot tablo)
		[ValidateNever]
		public ICollection<AntrenorHizmet>? AntrenorHizmetler { get; set; }


	}
}
