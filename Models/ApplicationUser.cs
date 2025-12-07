using Microsoft.AspNetCore.Identity;

namespace FitnessCenterReservationSystem.Models
{
	public class ApplicationUser:IdentityUser
	{
		public ICollection<Randevu>?Randevular{ get; set; }
	}
}
