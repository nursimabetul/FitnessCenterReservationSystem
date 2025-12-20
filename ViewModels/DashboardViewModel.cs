namespace FitnessCenterReservationSystem.ViewModels
{
	public class DashboardViewModel
	{
		// Randevu istatistikleri
		public int AktifRandevular { get; set; }
		public int Bekleyen { get; set; }
		public int Onaylanan { get; set; }
		public int Gecmis { get; set; }
		public int Tamamlanan { get; set; }
		public int Reddedilen { get; set; }
		public int IptalEdilen { get; set; }


		// Randevu listeleri
		public List<RandevularListViewModel> BugunRandevular { get; set; }
		public List<RandevularListViewModel> YaklasanRandevular { get; set; }
	}
}
