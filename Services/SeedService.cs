using FitnessCenterReservationSystem.Data;
using FitnessCenterReservationSystem.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace FitnessCenterReservationSystem.Services
{
 	 public class SeedService
	{
		public static async Task SeedDatabase(IServiceProvider serviceProvider)
		{
			using var scope = serviceProvider.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext >();
			var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedService>>();

			try
			{
				// Veritabanının hazır olduğundan emin ol
				logger.LogInformation("Veritabanı oluşturuluyor veya hazır olduğu kontrol ediliyor.");
				await context.Database.EnsureCreatedAsync();

				// Rolleri ekle
				logger.LogInformation("Roller ekleniyor.");
				await AddRoleAsync(roleManager, "Admin");
				await AddRoleAsync(roleManager, "Antrenör");
				await AddRoleAsync(roleManager, "Üye");
				// Admin kullanıcı ekle
				logger.LogInformation("Admin kullanıcı ekleniyor.");
				var adminEmail = "G211210553@sakarya.edu.tr";
				if (await userManager.FindByEmailAsync(adminEmail) == null)
				{
					var adminUser = new ApplicationUser
					{
						Ad = "Nursima Betül",
						Soyad="BALLİ",
						DogumTarihi= new DateTime(2004, 5, 12),
						UserName = adminEmail,
						Boy=175,
						Kilo=75,
						NormalizedUserName = adminEmail.ToUpper(),
						Email = adminEmail,
						NormalizedEmail = adminEmail.ToUpper(),
						EmailConfirmed = true,
						Onaylandi = true,
						SecurityStamp = Guid.NewGuid().ToString()
					};
					
					var result = await userManager.CreateAsync(adminUser, "sau");
					if (result.Succeeded)
					{
						logger.LogInformation("Admin rolü admin kullanıcıya atandı."); 
						await userManager.AddToRoleAsync(adminUser, "Admin");
					}
					else
					{
						logger.LogError("Admin kullanıcı oluşturulamadı: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
					}
				}


				// Üye kullanıcı ekle
				logger.LogInformation("Üye kullanıcı ekleniyor.");
				var uyeEmail = "betul@sakarya.edu.tr";

				if (await userManager.FindByEmailAsync(uyeEmail) == null)
				{
					var uyeUser = new ApplicationUser
					{
						Ad = "Betül",
						Soyad = "BALLİ",
						DogumTarihi = new DateTime(2004, 6, 20),
						Boy = 176,
						Kilo = 76,
						UserName = uyeEmail,
						NormalizedUserName = uyeEmail.ToUpper(),
						Email = uyeEmail,
						NormalizedEmail = uyeEmail.ToUpper(),
						EmailConfirmed = true,
						Onaylandi = true,
						SecurityStamp = Guid.NewGuid().ToString()
					};

					var result = await userManager.CreateAsync(uyeUser, "sau"); // şifre aynı
					if (result.Succeeded)
					{
						logger.LogInformation("Üye rolü üye kullanıcıya atandı.");
						await userManager.AddToRoleAsync(uyeUser, "Üye");
					}
					else
					{
						logger.LogError("Üye kullanıcı oluşturulamadı: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
					}
				}

			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Veritabanı oluşturulurken bir hata oluştu.");
			}

		}

		private static async Task AddRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
		{
			if (!await roleManager.RoleExistsAsync(roleName))
			{
				var result = await roleManager.CreateAsync(new IdentityRole(roleName));
				if (!result.Succeeded)
				{
					throw new Exception($"'{roleName}' rolü oluşturulamadı: {string.Join(", ", result.Errors.Select(e => e.Description))}");
				}
			}
		}
	}
}
