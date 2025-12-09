namespace FitnessCenterReservationSystem.Models
{
	public class AntrenorCalismaSaati
	{
		public int Id { get; set; }

		// Hangi antrenöre ait
		public string AntrenorId { get; set; }
		public ApplicationUser? Antrenor { get; set; }

		// Haftanın günü
		public DayOfWeek Gun { get; set; }

		// Çalışma saat aralığı
		public TimeSpan BaslangicSaati { get; set; }
		public TimeSpan BitisSaati { get; set; }
	}
}
