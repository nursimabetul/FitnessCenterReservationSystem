namespace FitnessCenterReservationSystem.ViewModels
{
	public class AntrenorHizmetViewModel
	{
		public string AntrenorId { get; set; }
		public string? AdSoyad { get; set; }
		public string? Email { get; set; }

		public List<HizmetCheckboxVM> TumHizmetler { get; set; } = new();
	}

	public class HizmetCheckboxVM
	{
 			public int Id { get; set; }
			public string? Ad { get; set; }

			public int SureDakika { get; set; }
			public decimal Ucret { get; set; }
			public string? SalonAdi { get; set; }

			public bool Secili { get; set; }
		}

	}
 
