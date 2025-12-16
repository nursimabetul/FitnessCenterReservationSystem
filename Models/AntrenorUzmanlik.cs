using System.ComponentModel.DataAnnotations;

namespace FitnessCenterReservationSystem.Models
{
	public class AntrenorUzmanlik
	{
		public int Id { get; set; }

		// Antrenör Id (ApplicationUser)
		[Required(ErrorMessage = "Antrenör seçimi zorunludur.")]
		[Display(Name = "Antrenör")]
		public string? AntrenorId { get; set; }

		public ApplicationUser? Antrenor { get; set; }

		// Uzmanlık Alanı
		[Required(ErrorMessage = "Uzmanlık alanı seçimi zorunludur.")]
		[Display(Name = "Uzmanlık Alanı")]
		public int UzmanlikAlaniId { get; set; }

		public UzmanlikAlani? UzmanlikAlani { get; set; }
	}
}