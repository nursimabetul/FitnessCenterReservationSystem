namespace FitnessCenterReservationSystem.Models
{
	public class Randevu
	{

		public int RandevuId { get; set; }
		public DateTime Tarih { get; set; }
		// başlangıç saati
		public TimeSpan BaslangicSaati { get; set; }
		// bitiş saati

		public TimeSpan BitisSaati { get; set; }
		// Üye
		public string? UyeId { get; set; }
		public ApplicationUser? Uye { get; set; }

		// Antrenör
		public string? AntrenorId { get; set; }
		public ApplicationUser? Antrenor { get; set; }

		// Salon
		public int SalonId { get; set; }
		public Salon? Salon { get; set; }

		// Hizmet
		public int HizmetId { get; set; }
		public Hizmet? Hizmet { get; set; }











	}
}