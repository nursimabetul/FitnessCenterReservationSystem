namespace FitnessCenterReservationSystem.Models
{
	public class SalonCalismaSaati
	{
		public int Id { get; set; }

		public int SalonId { get; set; }
		public Salon Salon { get; set; }

		public DayOfWeek Gun { get; set; }  // Pazartesi, Salı, Çarşamba...

		public TimeSpan AcilisSaati { get; set; }
		public TimeSpan KapanisSaati { get; set; }
	}
}
