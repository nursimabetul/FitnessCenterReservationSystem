using FitnessCenterReservationSystem.Models;
using FitnessCenterReservationSystem.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace FitnessCenterReservationSystem.Extensions
{
	public static class RandevuHtmlExtensions
	{
		public static IHtmlContent RandevuIslemButtons(this IHtmlHelper html, RandevularListViewModel r)
		{
			var buttons = "";

			if (r.UserRole == "Admin" || r.UserRole == "Antrenör")
			{
				if (r.Durum == RandevuDurum.Beklemede)
				{
					buttons += $"<a class='btn btn-sm btn-success me-1' href='/Antrenor/Approve/{r.Id}' title='Onayla'><i class='bi bi-check-lg'></i></a>";
					buttons += $"<a class='btn btn-sm btn-danger me-1' href='/Antrenor/Reject/{r.Id}' title='Reddet'><i class='bi bi-x-lg'></i></a>";
					buttons += $"<a class='btn btn-sm btn-warning' href='/Antrenor/Iptal/{r.Id}' title='İptal'><i class='bi bi-x-circle'></i></a>";
				}
				else if (r.Durum == RandevuDurum.Onaylandi && r.Tarih < DateTime.Today)
				{
					buttons += $"<a class='btn btn-sm btn-primary' href='/Antrenor/Tamamla/{r.Id}' title='Tamamla'><i class='bi bi-check-circle'></i></a>";
					buttons += $"<a class='btn btn-sm btn-warning' href='/Antrenor/Iptal/{r.Id}' title='İptal'><i class='bi bi-x-circle'></i></a>";
				}
				else if (r.Durum == RandevuDurum.Tamamlandi)
				{
					buttons += "<span class='text-muted'>Tamamlandı</span>";
				}
			}
			else if (r.UserRole == "Üye")
			{
				if (r.Durum == RandevuDurum.Beklemede || r.Durum == RandevuDurum.Onaylandi)
				{
					buttons += $"<a class='btn btn-sm btn-warning' href='/Uye/Iptal/{r.Id}' title='İptal'><i class='bi bi-x-circle'></i></a>";
				}
				else if (r.Durum == RandevuDurum.Tamamlandi)
				{
					buttons += "<span class='text-muted'>Tamamlandı</span>";
				}
			}

			return new HtmlString(buttons);
		}
	}
}
