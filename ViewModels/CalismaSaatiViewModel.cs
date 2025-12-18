using System.ComponentModel.DataAnnotations;

namespace FitnessCenterReservationSystem.ViewModels
{
	public class CalismaSaatiViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Antrenör seçilmelidir.")]
		[Display(Name = "Antrenör")]
		public string AntrenorId { get; set; }

		[Required(ErrorMessage = "Gün seçilmelidir.")]
		[Display(Name = "Haftanın Günü")]
		public DayOfWeek Gun { get; set; }

		[Required(ErrorMessage = "Başlangıç saati girilmelidir.")]
		[DataType(DataType.Time)]
		[Display(Name = "Başlangıç Saati")]
		public TimeSpan BaslangicSaati { get; set; }

		[Required(ErrorMessage = "Bitiş saati girilmelidir.")]
		[DataType(DataType.Time)]
		[Display(Name = "Bitiş Saati")]
		public TimeSpan BitisSaati { get; set; }

		public string? AntrenorAdi { get; set; } // Listeleme için
	}
}
