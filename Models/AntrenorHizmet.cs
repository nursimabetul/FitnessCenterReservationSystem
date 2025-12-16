using System.ComponentModel.DataAnnotations;

namespace FitnessCenterReservationSystem.Models
{
	public class AntrenorHizmet
	{
		public int Id { get; set; }

		// Antrenör Id (ApplicationUser)
		[Required(ErrorMessage = "Antrenör seçimi zorunludur.")]
		[Display(Name = "Antrenör")]
		public string? AntrenorId { get; set; }

		public ApplicationUser? Antrenor { get; set; }

		// Hizmet
		[Required(ErrorMessage = "Hizmet seçimi zorunludur.")]
		[Display(Name = "Hizmet")]
		public int HizmetId { get; set; }

		public Hizmet? Hizmet { get; set; }
	}
}
