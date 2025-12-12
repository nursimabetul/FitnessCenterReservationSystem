namespace FitnessCenterReservationSystem.Models
{
	public class UzmanlikAlani
	{
		public int Id { get; set; }   
		public string Ad { get; set; } = string.Empty;

		public ICollection<AntrenorUzmanlik> AntrenorUzmanliklar { get; set; }
			= new List<AntrenorUzmanlik>();
	}
}
