namespace FitnessCenterReservationSystem.Models
{
	public class AntrenorHizmet
	{
		public int AntrenorHizmetId { get; set; }

		// Antrenör Id (ApplicationUser)
		public string? AntrenorId { get; set; }
		public ApplicationUser? Antrenor { get; set; }

		// Hizmet
		public int HizmetId { get; set; }
		public Hizmet? Hizmet { get; set; }
	}
}
