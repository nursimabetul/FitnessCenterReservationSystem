using Microsoft.AspNetCore.Identity;

namespace FitnessCenterReservationSystem.Models
{
	public class ApplicationUser:IdentityUser
	{
		public string? Ad { get; set; }
		public string? Soyad { get; set; }
		public DateTime? DogumTarihi { get; set; }
		public double? Boy { get; set; }
		public double? Kilo { get; set; }

		// Üyenin aldığı randevular
		public ICollection<Randevu>? UyeRandevulari { get; set; }

		// Antrenörün baktığı randevular
		public ICollection<Randevu>? AntrenorRandevulari { get; set; }

		// Antrenörün verebildiği hizmetler (Many-to-Many)
		public ICollection<AntrenorHizmet>? AntrenorHizmetler { get; set; }



	}
}
