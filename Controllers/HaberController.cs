using FitnessCenterReservationSystem.Data;
using FitnessCenterReservationSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessCenterReservationSystem.Controllers
{
    public class HaberController : Controller
    {
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public HaberController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}
		// GET: Haber
		public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Haberler.Include(h => h.Olusturan).Include(h => h.Salon);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Haber/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var haber = await _context.Haberler
                .Include(h => h.Olusturan)
                .Include(h => h.Salon)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (haber == null)
            {
                return NotFound();
            }

            return View(haber);
        }

        // GET: Haber/Create
        public IActionResult Create()
        {
            ViewData["OlusturanId"] = new SelectList(_context.Users,  "Id", "Ad");
            ViewData["SalonId"] = new SelectList(_context.Salonlar, "Id", "Ad");
            return View();
        }

        // POST: Haber/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Baslik,Icerik,Tarih,SalonId,OlusturanId,ResimDosyasi")] Haber haber)
		{
			if (ModelState.IsValid)
			{
				if (haber.ResimDosyasi != null && haber.ResimDosyasi.Length > 0)
				{
					var fileName = Guid.NewGuid() + System.IO.Path.GetExtension(haber.ResimDosyasi.FileName);
					var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/haberler", fileName);

					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await haber.ResimDosyasi.CopyToAsync(stream);
					}

					haber.ResimYolu = "/images/haberler/" + fileName;
				}

				_context.Add(haber);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			ViewData["OlusturanId"] = new SelectList(_context.Users, "Id", "Ad", haber.OlusturanId);
			ViewData["SalonId"] = new SelectList(_context.Salonlar, "Id", "Ad", haber.SalonId);
			return View(haber);
		}

		// GET: Haber/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var haber = await _context.Haberler.FindAsync(id);
            if (haber == null)
            {
                return NotFound();
            }
            ViewData["OlusturanId"] = new SelectList(_context.Users, "Id", "Ad", haber.OlusturanId);
            ViewData["SalonId"] = new SelectList(_context.Salonlar, "Id", "Ad", haber.SalonId);
            return View(haber);
        }

        // POST: Haber/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Baslik,Icerik,Tarih,SalonId,OlusturanId")] Haber haber, IFormFile? ResimDosyasi)
		{
			if (id != haber.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					// Mevcut resmi güncelle
					if (ResimDosyasi != null && ResimDosyasi.Length > 0)
					{
						var folder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "haberler");
						if (!Directory.Exists(folder))
							Directory.CreateDirectory(folder);

						// Benzersiz isim
						var fileName = Guid.NewGuid() + Path.GetExtension(ResimDosyasi.FileName);
						var filePath = Path.Combine(folder, fileName);

						// Dosyayı kaydet
						using (var stream = new FileStream(filePath, FileMode.Create))
						{
							await ResimDosyasi.CopyToAsync(stream);
						}

						// Eski resmi sil (isteğe bağlı)
						if (!string.IsNullOrEmpty(haber.ResimYolu))
						{
							var eskiDosya = Path.Combine(_webHostEnvironment.WebRootPath, haber.ResimYolu.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
							if (System.IO.File.Exists(eskiDosya))
								System.IO.File.Delete(eskiDosya);
						}

						haber.ResimYolu = "/images/haberler/" + fileName;
					}

					_context.Update(haber);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!HaberExists(haber.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}

			ViewData["OlusturanId"] = new SelectList(_context.Users, "Id", "Ad", haber.OlusturanId);
			ViewData["SalonId"] = new SelectList(_context.Salonlar, "Id", "Ad", haber.SalonId);
			return View(haber);
		}

		// GET: Haber/Delete/5
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var haber = await _context.Haberler
                .Include(h => h.Olusturan)
                .Include(h => h.Salon)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (haber == null)
            {
                return NotFound();
            }

            return View(haber);
        }

        // POST: Haber/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var haber = await _context.Haberler.FindAsync(id);
            if (haber != null)
            {
                _context.Haberler.Remove(haber);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HaberExists(int id)
        {
            return _context.Haberler.Any(e => e.Id == id);
        }

		// GET: /Haber/HaberlerCard
		[Authorize(Roles = "Üye,Antrenör")]
		public async Task<IActionResult> HaberlerCard()
		{
			var haberler = await _context.Haberler
				.Include(h => h.Olusturan)
				.Include(h => h.Salon)
				.OrderByDescending(h => h.Tarih)
				.ToListAsync();

			return View(haberler);
		}
	}
}
