using Microsoft.AspNetCore.Identity;

namespace FitnessCenterReservationSystem.Models
{
	public class ApplicationUser:IdentityUser
	{
		public string? Ad { get; set; }
		public string? Soyad { get; set; }
		public DateTime? DogumTarihi { get; set; } //= DateTime.UtcNow;
		public double? Boy { get; set; }
		public double? Kilo { get; set; }

		// Üyenin aldığı randevular
		public ICollection<Randevu>? UyeRandevulari { get; set; }

		// Antrenörün baktığı randevular
		public ICollection<Randevu>? AntrenorRandevulari { get; set; }



		// --- Antrenörün verebildiği hizmetler ---
		public ICollection<AntrenorHizmet>? AntrenorHizmetler { get; set; }
		// --- Antrenörün uzmanlık Alanlari---
		public ICollection<AntrenorUzmanlik>? AntrenorUzmanlikAlanlari { get; set; }

		//Antrenör çalışma saatleri
		public ICollection<AntrenorCalismaSaati>? CalismaSaatleri { get; set; }

		public int? SalonId { get; set; }
		public Salon? Salon { get; set; }
		// Kullanıcı onay durumu
		public bool Onaylandi { get; set; } = false; // Başlangıçta onay beklemede

	}
}
