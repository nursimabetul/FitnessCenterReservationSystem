namespace FitnessCenterReservationSystem.Models
{
	public enum		RandevuDurum
	{
		Beklemede = 0,
		Onaylandi = 1,    // Gelecek randevu
		Tamamlandi = 2,  // Gerçekleşti
		Reddedildi = 3,
		Iptal = 4
	}
}
