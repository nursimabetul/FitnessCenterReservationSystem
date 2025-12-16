using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FitnessCenterReservationSystem.ViewModels
{
	public class ProfileViewModel
	{
		public string Id { get; set; }

		[Display(Name = "Ad")]
		public string? Ad { get; set; }

		[Display(Name = "Soyad")]
		public string? Soyad { get; set; }

		[Display(Name = "Email")]
		public string? Email { get; set; }

		[Display(Name = "Doğum Tarihi")]
		[DataType(DataType.Date)]
		public DateTime? DogumTarihi { get; set; }

		[Display(Name = "Boy (cm)")]
		public double? Boy { get; set; }

		[Display(Name = "Kilo (kg)")]
		public double? Kilo { get; set; }

		// Sadece Antrenörler için
		[Display(Name = "Çalıştığı Salon")]
		public int? SalonId { get; set; }
		public List<SelectListItem>? Salonlar { get; set; }

		// Sadece Antrenörler için
		[Display(Name = "Uzmanlık Alanları")]
		public List<int>? SecilenUzmanliklar { get; set; }
		public List<SelectListItem>? UzmanlikAlanlari { get; set; }
	}
}
