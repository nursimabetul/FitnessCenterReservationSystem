using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FitnessCenterReservationSystem.Models
{
	public class ApplicationUser:IdentityUser
	{
		[Display(Name = "Ad")]
		public string? Ad { get; set; }

		[Display(Name = "Soyad")]
		public string? Soyad { get; set; }

		[Display(Name = "Doğum Tarihi")]
		[DataType(DataType.Date)]
		public DateTime? DogumTarihi { get; set; }

		[Display(Name = "Boy (cm)")]
		[Range(0, 300, ErrorMessage = "Lütfen geçerli bir boy giriniz.")]
		public double? Boy { get; set; }

		[Display(Name = "Kilo (kg)")]
		[Range(0, 500, ErrorMessage = "Lütfen geçerli bir kilo giriniz.")]
		public double? Kilo { get; set; }

		// Üyenin aldığı randevular
		public ICollection<Randevu>? UyeRandevulari { get; set; }

		// Antrenörün baktığı randevular
		public ICollection<Randevu>? AntrenorRandevulari { get; set; }

		// --- Antrenörün verebildiği hizmetler ---
		public ICollection<AntrenorHizmet>? AntrenorHizmetler { get; set; }

		// --- Antrenörün uzmanlık Alanları ---
		public ICollection<AntrenorUzmanlik>? AntrenorUzmanlikAlanlari { get; set; }

		// Antrenör çalışma saatleri
		public ICollection<AntrenorCalismaSaati>? CalismaSaatleri { get; set; }

		// Hangi salona bağlı (isteğe bağlı)
		public int? SalonId { get; set; }
		public Salon? Salon { get; set; }

		// Kullanıcı onay durumu
		[Display(Name = "Onaylandı")]
		public bool Onaylandi { get; set; } = false; // Başlangıçta onay beklemede

		public bool AktifMi { get; set; } = true; // varsayılan aktif
	}
}
