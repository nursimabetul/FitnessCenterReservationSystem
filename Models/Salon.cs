using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FitnessCenterReservationSystem.Models
{
	public class Salon
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Salon adı zorunludur.")]
		[StringLength(100, ErrorMessage = "Salon adı en fazla 100 karakter olabilir.")]
		[Display(Name = "Salon Adı")]
		public string Ad { get; set; } = string.Empty;

		[Required(ErrorMessage = "Adres zorunludur.")]
		[StringLength(250, ErrorMessage = "Adres en fazla 250 karakter olabilir.")]
		[Display(Name = "Adres")]
		public string Adres { get; set; } = string.Empty;

		[Required(ErrorMessage = "Telefon numarası zorunludur.")]
		[Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
		[Display(Name = "Telefon")]
		public string telefon { get; set; } = string.Empty;

		[Required(ErrorMessage = "Açılış saati zorunludur.")]
		[DataType(DataType.Time)]
		[Display(Name = "Açılış Saati")]
		public TimeSpan AcilisSaati { get; set; }

		[Required(ErrorMessage = "Kapanış saati zorunludur.")]
		[DataType(DataType.Time)]
		[Display(Name = "Kapanış Saati")]
		public TimeSpan KapanisSaati { get; set; }

		[ValidateNever]
		public ICollection<Hizmet> Hizmetler { get; set; } = new List<Hizmet>();

		[ValidateNever]
		public ICollection<Randevu> Randevular { get; set; } = new List<Randevu>();

		[ValidateNever]
		public ICollection<Kampanya> Kampanyalar { get; set; } = new List<Kampanya>();
	}
}
