namespace FitnessCenterReservationSystem.ViewModels
{
	public class AntrenorUzmanlikViewModel
	{
		// Antrenör bilgileri
		public string AntrenorId { get; set; }
		public string AdSoyad { get; set; }
		public string Email { get; set; }

		// Index için (gösterim)
		public List<string> Uzmanliklar { get; set; } = new();

		// Edit için (form)
		public List<int> SecilenUzmanlikler { get; set; } = new();
		public List<UzmanlikCheckboxVM> TumUzmanliklar { get; set; } = new();
	}

	public class UzmanlikCheckboxVM
	{
		public int Id { get; set; }
		public string Ad { get; set; }
		public bool Secili { get; set; }
	}
}
