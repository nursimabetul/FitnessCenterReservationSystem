using FitnessCenterReservationSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace FitnessCenterReservationSystem.ViewModels
{
	public class RandevuViewModel
	{
		public int Id { get; set; }

		public string UyeId { get; set; }
		public string AntrenorId { get; set; }
		public int HizmetId { get; set; }
		public int SalonId { get; set; }

		public DateTime Tarih { get; set; }
		public TimeSpan BaslangicSaati { get; set; }
		public TimeSpan BitisSaati { get; set; }

		public RandevuDurum Durum { get; set; }

		// Dropdownlar
		public IEnumerable<SelectListItem> Uyeler { get; set; }
		public IEnumerable<SelectListItem> Antrenorler { get; set; }
		public IEnumerable<SelectListItem> Hizmetler { get; set; }
		public IEnumerable<SelectListItem> Salonlar { get; set; }
	}

	 
}