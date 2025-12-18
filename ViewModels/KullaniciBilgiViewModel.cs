using System.ComponentModel.DataAnnotations;

namespace FitnessCenterReservationSystem.ViewModels
{
	public class KullaniciBilgiViewModel
	{
		[Display(Name = "Boy (cm)")]
		[Range(0, 300)]
		public double? Boy { get; set; }

		[Display(Name = "Kilo (kg)")]
		[Range(0, 500)]
		public double? Kilo { get; set; }

		[Display(Name = "Doğum Tarihi")]
		[DataType(DataType.Date)]
		public DateTime? DogumTarihi { get; set; }

		[Display(Name = "Hedef")]
		[Required]
		public string Hedef { get; set; } // Örn: Kilo verme, Kas geliştirme, Formda kalma
	}
}