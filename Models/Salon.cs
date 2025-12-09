namespace FitnessCenterReservationSystem.Models
{
	public class Salon
	{
		public int Id { get; set; }
		public string Ad { get; set; } = string.Empty;

		public ICollection<SalonCalismaSaati> CalismaSaatleri { get; set; }
			= new List<SalonCalismaSaati>();

		public ICollection<SalonHizmet> SalonHizmetleri { get; set; }
			= new List<SalonHizmet>();

		public ICollection<Randevu> Randevular { get; set; }
			= new List<Randevu>();
	}
}
}
