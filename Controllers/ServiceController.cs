using FitnessCenterApp.Data; 
using FitnessCenterApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Service
        public async Task<IActionResult> Index()
        {
            var services = await _context.Services
                .Include(s => s.Gym)
                .ToListAsync();

            return View(services);
        }

        // GET: Service/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var service = await _context.Services
                .Include(s => s.Gym)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (service == null) return NotFound();

            return View(service);
        }

        // GET: Service/Create
        public async Task<IActionResult> Create()
        {
            await LoadGymsDropDownList();
            return View();
        }

       [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(Service service)
{
    // فحص: هل يوجد خدمة بنفس الاسم في نفس الصالة؟
    bool exists = await _context.Services
        .AnyAsync(s => s.Name == service.Name && s.GymId == service.GymId);

    if (exists)
    {
        ModelState.AddModelError(string.Empty,
            "Bu spor salonunda aynı isimde bir hizmet zaten kayıtlı.");
    }

    if (!ModelState.IsValid)
    {
        await LoadGymsDropDownList(service.GymId);
        return View(service);
    }

    _context.Add(service);
    await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
}


        // GET: Service/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();

            await LoadGymsDropDownList(service.GymId);
            return View(service);
        }

        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, Service service)
{
    if (id != service.Id) return NotFound();

    // فحص: هل يوجد خدمة أخرى (غير الحالية) بنفس الاسم في نفس الصالة؟
    bool exists = await _context.Services
        .AnyAsync(s => s.Id != service.Id &&
                       s.Name == service.Name &&
                       s.GymId == service.GymId);

    if (exists)
    {
        ModelState.AddModelError(string.Empty,
            "Bu spor salonunda aynı isimde bir hizmet zaten kayıtlı.");
    }

    if (!ModelState.IsValid)
    {
        await LoadGymsDropDownList(service.GymId);
        return View(service);
    }

    try
    {
        _context.Update(service);
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!ServiceExists(service.Id))
            return NotFound();
        else
            throw;
    }

    return RedirectToAction(nameof(Index));
}


        // GET: Service/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var service = await _context.Services
                .Include(s => s.Gym)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (service == null) return NotFound();

            return View(service);
        }

        // POST: Service/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }

        private async Task LoadGymsDropDownList(object selectedGym = null)
        {
            var gyms = await _context.Gyms
                .OrderBy(g => g.Name)
                .ToListAsync();

            ViewBag.GymId = new SelectList(gyms, "Id", "Name", selectedGym);
        }
    }
}
