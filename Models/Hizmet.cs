namespace FitnessCenterReservationSystem.Models
{
	public class Hizmet
	{
		public int Id { get; set; }
		public string? Ad { get; set; }
		public int SureDakika { get; set; }
		public decimal Ucret { get; set; }

		public int SalonId { get; set; }
		public Salon Salon { get; set; }


		// Bu hizmete ait randevular
		public ICollection<Randevu>? Randevular { get; set; }

		// Hizmeti verebilen antrenörler (pivot tablo)
		public ICollection<AntrenorHizmet> AntrenorHizmetler { get; set; }

	}
}
