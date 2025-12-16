using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FitnessCenterReservationSystem.Models
{
	public class Kampanya
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Kampanya başlığı zorunludur.")]
		[StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir.")]
		public string? Baslik { get; set; }

		[Required(ErrorMessage = "Kampanya açıklaması zorunludur.")]
		[StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
		public string? Aciklama { get; set; }

		// Kampanya başlangıç ve bitiş tarihleri
		[DataType(DataType.Date)]
		public DateTime BaslangicTarihi { get; set; }

		[DataType(DataType.Date)]
		public DateTime BitisTarihi { get; set; }

		// Opsiyonel: Kampanya hangi salon veya hizmete ait
		public int? SalonId { get; set; }

		[ValidateNever]
		public Salon? Salon { get; set; }

		public int? HizmetId { get; set; }

		[ValidateNever]
		public Hizmet? Hizmet { get; set; }
	}
}
