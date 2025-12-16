using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FitnessCenterReservationSystem.Models
{
	public class Randevu
	{

		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Randevu tarihi zorunludur.")]
		[DataType(DataType.Date)]
		[Display(Name = "Randevu Tarihi")]
		public DateTime Tarih { get; set; }

		[Required(ErrorMessage = "Başlangıç saati girilmelidir.")]
		[DataType(DataType.Time)]
		[Display(Name = "Başlangıç Saati")]
		public TimeSpan BaslangicSaati { get; set; }

		[Required(ErrorMessage = "Bitiş saati girilmelidir.")]
		[DataType(DataType.Time)]
		[Display(Name = "Bitiş Saati")]
		public TimeSpan BitisSaati { get; set; }

		// Üye
		[Required(ErrorMessage = "Üye seçilmelidir.")]
		[Display(Name = "Üye")]
		public string? UyeId { get; set; }

		[ValidateNever]
		public ApplicationUser? Uye { get; set; }

		// Antrenör
		[Required(ErrorMessage = "Antrenör seçilmelidir.")]
		[Display(Name = "Antrenör")]
		public string? AntrenorId { get; set; }

		[ValidateNever]
		public ApplicationUser? Antrenor { get; set; }

		// Salon
		[Required(ErrorMessage = "Salon seçilmelidir.")]
		[Display(Name = "Salon")]
		public int SalonId { get; set; }

		[ValidateNever]
		public Salon? Salon { get; set; }

		// Hizmet
		[Required(ErrorMessage = "Hizmet seçilmelidir.")]
		[Display(Name = "Hizmet")]
		public int HizmetId { get; set; }

		[ValidateNever]
		public Hizmet? Hizmet { get; set; }

		[Display(Name = "Durum")]
		public RandevuDurum Durum { get; set; } = RandevuDurum.Beklemede;


	}


}