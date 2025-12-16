using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FitnessCenterReservationSystem.Models
{
	public class UzmanlikAlani
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "Uzmanlık alanı adı zorunludur.")]
		[StringLength(100, ErrorMessage = "Uzmanlık alanı adı en fazla 100 karakter olabilir.")]
		[Display(Name = "Uzmanlık Alanı")]
		public string Ad { get; set; } = string.Empty;

		[ValidateNever]
		public ICollection<AntrenorUzmanlik> AntrenorUzmanliklar { get; set; }
			= new List<AntrenorUzmanlik>();
	}
}
