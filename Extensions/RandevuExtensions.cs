using FitnessCenterReservationSystem.Models;
using FitnessCenterReservationSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitnessCenterReservationSystem.Extensions
{
	public static class RandevuExtensions
	{
		// IEnumerable<Randevu> kullanıyoruz ki List veya başka koleksiyon fark etmesin
		public static List<RandevularListViewModel> ToRandevularListViewModel(this IEnumerable<Randevu> randevular, string role)
		{
			if (randevular == null) return new List<RandevularListViewModel>();

			return randevular.Select(r => new RandevularListViewModel
			{
				Id = r.Id,
				UyeAdSoyad = r.Uye != null ? $"{r.Uye.Ad} {r.Uye.Soyad}" : "",
				AnrenörAdSoyad = r.Antrenor != null ? $"{r.Antrenor.Ad} {r.Antrenor.Soyad}" : "",
				HizmetAd = r.Hizmet?.Ad ?? "",
				SalonAd = r.Salon?.Ad ?? "",
				Ucret = r.Hizmet?.Ucret ?? 0,
				Tarih = r.Tarih,
				BaslangicSaati = r.BaslangicSaati,
				BitisSaati = r.BitisSaati,
				Durum = r.Durum,
				DurumIcon = r.Durum switch
				{
					RandevuDurum.Beklemede => "bi-hourglass-split text-warning",
					RandevuDurum.Onaylandi => "bi-check-circle text-success",
					RandevuDurum.Tamamlandi => "bi-check-lg text-primary",
					RandevuDurum.Iptal => "bi-x-circle text-danger",
					RandevuDurum.Reddedildi => "bi-x-circle text-secondary",
					_ => "bi-question-circle"
				},
				DurumLabel = r.Durum.ToString(),
				UserRole = role
			}).ToList();
		}
	}
}
