namespace FitnessCenterReservationSystem.Models
{
	public class AntrenorUzmanlik
	{
		public int AntrenorUzmanlikAlaniId { get; set; }

		public string? AntrenorId { get; set; }
		public ApplicationUser? Antrenor { get; set; }

		public int UzmanlikAlaniId { get; set; }
		public UzmanlikAlani? UzmanlikAlani { get; set; }
	}
}