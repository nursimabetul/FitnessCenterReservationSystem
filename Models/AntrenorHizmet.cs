namespace FitnessCenterReservationSystem.Models
{
	public class AntrenorHizmet
	{
		public int AntrenorHizmetId { get; set; }

		public string AntrenorId { get; set; }
		public ApplicationUser? Antrenor { get; set; }

		public int HizmetId { get; set; }
		public Hizmet? Hizmet { get; set; }
	}
}