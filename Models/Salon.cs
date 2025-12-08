namespace FitnessCenterReservationSystem.Models
{
	public class Salon
	{
		public int SalonId { get; set; }
		public string Ad { get; set; }
		public string? Adres { get; set; }
		public string? CalismaSaatleri { get; set; }

		// Salonun sunduğu hizmetler
		public ICollection<Hizmet>? Hizmetler { get; set; }

		// Salonun antrenörleri
		public ICollection<ApplicationUser>? Antrenorler { get; set; }

		// Bu salona ait randevular
		public ICollection<Randevu>? Randevular { get; set; }





		 





	}
}
