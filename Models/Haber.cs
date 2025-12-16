using System.ComponentModel.DataAnnotations;

namespace FitnessCenterReservationSystem.Models
{
	public class Haber
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		[Display(Name = "Başlık")]
		public string Baslik { get; set; } = string.Empty;

		[Required]
		[Display(Name = "İçerik")]
		public string Icerik { get; set; } = string.Empty;

		[Display(Name = "Yayın Tarihi")]
		public DateTime Tarih { get; set; } = DateTime.Now;

		// Hangi salona ait
		[Display(Name = "Salon")]
		public int? SalonId { get; set; }
		public Salon? Salon { get; set; }

		// Opsiyonel: Duyuruyu kim oluşturdu
		public string? OlusturanId { get; set; }
		public ApplicationUser? Olusturan { get; set; }
	}
}
