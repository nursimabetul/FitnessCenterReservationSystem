namespace FitnessCenterReservationSystem.Models
{
	public class Salon
	{
		public int Id { get; set; }
		public string Ad { get; set; } = string.Empty;

		public ICollection<SalonCalismaSaati> CalismaSaatleri { get; set; }
			= new List<SalonCalismaSaati>();

		public ICollection<Hizmet> Hizmetler { get; set; }
			= new List<Hizmet>();

		public ICollection<Randevu> Randevular { get; set; }
			= new List<Randevu>();
	}
}
