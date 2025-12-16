namespace FitnessCenterReservationSystem.Models
{
	public class Salon
	{
		public int Id { get; set; }
		public string Ad { get; set; } = string.Empty;
		public string Adres { get; set; } = string.Empty;
		public string telefon { get; set; } = string.Empty;

		public TimeSpan AcilisSaati { get; set; }
		public TimeSpan KapanisSaati { get; set; }

		public ICollection<Hizmet> Hizmetler { get; set; }
			= new List<Hizmet>();

		public ICollection<Randevu> Randevular { get; set; }
			= new List<Randevu>();
	}
}
