namespace FitnessCenterReservationSystem.Models
{
	public class Randevu
	{
		public int RandevuId { get; set; }
		public DateTime Tarih { get; set; }
		public string? ApplicationUserId { get; set; } //Foreign key
		public ApplicationUser? ApplicationUser { get; set; }   //Navigation property

	}
}