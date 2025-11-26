using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessCenterApp.Data;
using FitnessCenterApp.Models;

namespace FitnessCenterApp.Controllers
{
    public class GymController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GymController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var gyms = await _context.Gyms
                .Include(g => g.Services)
                .Include(g => g.Trainers)
                .ToListAsync();

            return View(gyms);
        }

        // GET: Gym/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var gym = await _context.Gyms
                .Include(g => g.Services)
                .Include(g => g.Trainers)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (gym == null) return NotFound();

            return View(gym);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Gym gym)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gym);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gym);
        }

        // GET: Gym/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var gym = await _context.Gyms.FindAsync(id);
            if (gym == null) return NotFound();

            return View(gym);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Gym gym)
        {
            if (id != gym.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gym);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymExists(gym.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gym);
        }

        // GET: Gym/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var gym = await _context.Gyms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gym == null) return NotFound();

            return View(gym);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gym = await _context.Gyms.FindAsync(id);
            if (gym != null)
            {
                _context.Gyms.Remove(gym);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool GymExists(int id)
        {
            return _context.Gyms.Any(e => e.Id == id);
        }
    }
}
