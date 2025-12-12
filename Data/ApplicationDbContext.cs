using FitnessCenterReservationSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterReservationSystem.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


		// --- DbSet Tanımları ---
		public DbSet<Salon> Salonlar { get; set; }
		public DbSet<AntrenorCalismaSaati> AntrenorCalismaSaatleri { get; set; }
		public DbSet<Hizmet> Hizmetler { get; set; }
		public DbSet<AntrenorHizmet> AntrenorHizmetler { get; set; }
		public DbSet<AntrenorUzmanlik> AntrenorUzmanlikAlanlari { get; set; }
		public DbSet<UzmanlikAlani> UzmanlikAlanlari { get; set; }
		public DbSet<Randevu> Randevular { get; set; }


		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// ------------------------------------------------------------
			// 1) ApplicationUser → Salon (FK)
			// ------------------------------------------------------------
			builder.Entity<ApplicationUser>()
				.HasOne(u => u.Salon)
				.WithMany()
				.HasForeignKey(u => u.SalonId)
				.OnDelete(DeleteBehavior.Restrict);

			// ------------------------------------------------------------
			// 2) ApplicationUser ↔ Randevu (Üye)
			// ------------------------------------------------------------
			builder.Entity<Randevu>()
				.HasOne(r => r.Uye)
				.WithMany(u => u.UyeRandevulari)
				.HasForeignKey(r => r.UyeId)
				.OnDelete(DeleteBehavior.Restrict);

			// ------------------------------------------------------------
			// 3) ApplicationUser ↔ Randevu (Antrenör)
			// ------------------------------------------------------------
			builder.Entity<Randevu>()
				.HasOne(r => r.Antrenor)
				.WithMany(a => a.AntrenorRandevulari)
				.HasForeignKey(r => r.AntrenorId)
				.OnDelete(DeleteBehavior.Restrict);

			// ------------------------------------------------------------
			// 4) Antrenor ↔ Hizmet (Many-to-Many)
			// ------------------------------------------------------------
			builder.Entity<AntrenorHizmet>()
				.HasKey(x => x.AntrenorHizmetId);

			builder.Entity<AntrenorHizmet>()
				.HasOne(x => x.Antrenor)
				.WithMany(a => a.AntrenorHizmetler)
				.HasForeignKey(x => x.AntrenorId)
				.OnDelete(DeleteBehavior.Restrict);



			// ------------------------------------------------------------
			// 5) Antrenör ↔ Uzmanlık Alanı (Many-to-Many)
			// ------------------------------------------------------------
			builder.Entity<AntrenorUzmanlik>()
				.HasKey(x => new { x.AntrenorId, x.UzmanlikAlaniId });

			builder.Entity<AntrenorUzmanlik>()
				.HasOne(x => x.Antrenor)
				.WithMany(a => a.AntrenorUzmanlikAlanlari)
				.HasForeignKey(x => x.AntrenorId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Entity<AntrenorUzmanlik>()
				.HasOne(x => x.UzmanlikAlani)
				.WithMany()
				.HasForeignKey(x => x.UzmanlikAlaniId)
				.OnDelete(DeleteBehavior.Cascade);

			// ------------------------------------------------------------
			// 6) Salon ↔ Hizmet (One-to-Many)
			// ------------------------------------------------------------
			builder.Entity<Hizmet>()
				.HasOne(h => h.Salon)
				.WithMany(s => s.Hizmetler)
				.HasForeignKey(h => h.SalonId)
				.OnDelete(DeleteBehavior.Cascade);

			// ------------------------------------------------------------
			// 7) Salon ↔ Çalışma Saatleri (One-to-Many)
			// ------------------------------------------------------------

			// !!! İptal edildi çünkü Salon çalışma saatleri modeli kullanılmıyor.

			// ------------------------------------------------------------
			// 8) Antrenör ↔ Çalışma Saatleri (One-to-Many)
			// ------------------------------------------------------------
			builder.Entity<AntrenorCalismaSaati>()
				.HasOne(a => a.Antrenor)
				.WithMany(u => u.CalismaSaatleri)
				.HasForeignKey(a => a.AntrenorId)
				.OnDelete(DeleteBehavior.Cascade);

			// ------------------------------------------------------------
			// 9) Salon ↔ Randevu
			// ------------------------------------------------------------
			builder.Entity<Randevu>()
				.HasOne(r => r.Salon)
				.WithMany(s => s.Randevular)
				.HasForeignKey(r => r.SalonId)
				.OnDelete(DeleteBehavior.Restrict);

			// ------------------------------------------------------------
			// 10) Hizmet ↔ Randevu
			// ------------------------------------------------------------
			builder.Entity<Randevu>()
				.HasOne(r => r.Hizmet)
				.WithMany(h => h.Randevular)
				.HasForeignKey(r => r.HizmetId)
				.OnDelete(DeleteBehavior.Restrict);
		}



	}
}
