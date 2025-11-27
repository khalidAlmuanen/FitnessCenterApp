using System.Linq;
using System.Threading.Tasks;
using FitnessCenterApp.Data;
using FitnessCenterApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TrainersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Trainers
        public async Task<IActionResult> Index()
        {
            var trainers = _context.Trainers
                                   .Include(t => t.Gym)
                                   .Include(t => t.Service);

            return View(await trainers.ToListAsync());
        }

        // GET: Trainers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var trainer = await _context.Trainers
                .Include(t => t.Gym)
                .Include(t => t.Service)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (trainer == null) return NotFound();

            return View(trainer);
        }

        // GET: Trainers/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Trainers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Trainer trainer)
        {
            // ✅ فحص: هل يوجد مدرّب بنفس الاسم في نفس الصالة؟
            bool exists = await _context.Trainers
                .AnyAsync(t => t.FullName == trainer.FullName &&
                               t.GymId == trainer.GymId);

            if (exists)
            {
                ModelState.AddModelError(string.Empty,
                    "Bu spor salonunda aynı isimde bir antrenör zaten kayıtlı.");
            }

            if (!ModelState.IsValid)
            {
                PopulateDropdowns(trainer.GymId, trainer.ServiceId);
                return View(trainer);
            }

            _context.Add(trainer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Trainers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null) return NotFound();

            PopulateDropdowns(trainer.GymId, trainer.ServiceId);
            return View(trainer);
        }

        // POST: Trainers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Trainer trainer)
        {
            if (id != trainer.Id) return NotFound();

            // ✅ فحص: هل يوجد مدرّب آخر بنفس الاسم في نفس الصالة؟
            bool exists = await _context.Trainers
                .AnyAsync(t => t.Id != trainer.Id &&
                               t.FullName == trainer.FullName &&
                               t.GymId == trainer.GymId);

            if (exists)
            {
                ModelState.AddModelError(string.Empty,
                    "Bu spor salonunda aynı isimde bir antrenör zaten kayıtlı.");
            }

            if (!ModelState.IsValid)
            {
                PopulateDropdowns(trainer.GymId, trainer.ServiceId);
                return View(trainer);
            }

            try
            {
                _context.Update(trainer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainerExists(trainer.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Trainers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var trainer = await _context.Trainers
                .Include(t => t.Gym)
                .Include(t => t.Service)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (trainer == null) return NotFound();

            return View(trainer);
        }

        // POST: Trainers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer != null)
            {
                _context.Trainers.Remove(trainer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TrainerExists(int id)
        {
            return _context.Trainers.Any(e => e.Id == id);
        }

        private void PopulateDropdowns(int? selectedGymId = null, int? selectedServiceId = null)
        {
            ViewData["GymId"] = new SelectList(_context.Gyms, "Id", "Name", selectedGymId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", selectedServiceId);
        }
    }
}
