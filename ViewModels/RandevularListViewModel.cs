using FitnessCenterReservationSystem.Models;

namespace FitnessCenterReservationSystem.ViewModels
{
	public class RandevularListViewModel
	{
		public int Id { get; set; }
		public string UyeAdSoyad { get; set; }
		public string AnrenörAdSoyad { get; set; }
		public string HizmetAd { get; set; }
		public decimal Ucret { get; set; }

		public string SalonAd { get; set; }
		public DateTime Tarih { get; set; }
		public TimeSpan BaslangicSaati { get; set; }
		public TimeSpan BitisSaati { get; set; }
		public RandevuDurum Durum { get; set; }

		// Dashboard için ikon veya etiket istersen eklenebilir
		public string DurumIcon { get; set; }
		public string DurumLabel { get; set; }
		public string UserRole { get; set; }
	}
}
