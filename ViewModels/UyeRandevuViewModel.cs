namespace FitnessCenterReservationSystem.ViewModels
{
	public class UyeRandevuViewModel
	{
		public DateTime Tarih { get; set; }
		public string BaslangicSaati { get; set; } = "";
		public string BitisSaati { get; set; } = "";
		public string Durum { get; set; } = "";
		public string Antrenor { get; set; } = "";
		public string Hizmet { get; set; } = "";
	}
}
