using Microsoft.AspNetCore.Mvc;
using FitnessCenterApp.Data;
using FitnessCenterApp.Models;

namespace FitnessCenterApp.ApiControllers
{
    [ApiController]
    [Route("api/trainers")]
    public class TrainersApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrainersApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /api/trainers
        [HttpGet]
        public IActionResult GetAll()
        {
            var trainers = _context.Trainers.ToList();
            return Ok(trainers);
        }

        // GET: /api/trainers?serviceId=1
        [HttpGet("filter")]
        public IActionResult Filter(int? serviceId)
        {
            var query = _context.Trainers.AsQueryable();

            if (serviceId.HasValue)
                query = query.Where(t => t.ServiceId == serviceId.Value);

            return Ok(query.ToList());
        }
    }
}
