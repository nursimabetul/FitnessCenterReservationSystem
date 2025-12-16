using System.ComponentModel.DataAnnotations;

namespace FitnessCenterReservationSystem.Models
{
	public class Duyuru
	{
		public int Id { get; set; }

		[StringLength(100)]
		[Display(Name = "Başlık")]
		[Required(ErrorMessage = "Başlık Alanının yazılması zorunludur.")]

		public string Baslik { get; set; } = string.Empty;

		[Required(ErrorMessage = "İçerik Alanının yazılması zorunludur.")]
		[Display(Name = "İçerik")]
		public string Icerik { get; set; } = string.Empty;

		[Display(Name = "Yayın Tarihi")]
		[Required(ErrorMessage = "Yayın Tarihinin seçilmesi zorunludur.")]

		public DateTime Tarih { get; set; } = DateTime.Now;

		// Hangi salona ait
		[Display(Name = "Salon")]
		public int? SalonId { get; set; }
		public Salon? Salon { get; set; }

		// Opsiyonel: Duyuruyu kim oluşturdu
		[Display(Name = "Duyuruyu Oluşturan Kullanıcı")]

		public string? OlusturanId { get; set; }
		public ApplicationUser? Olusturan { get; set; }
	}
}
