using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Filters;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ResultsController(AppDbContext context) { _context = context; }

        [HttpPost] // Organiser only
        [SessionAuthorize("Organiser")]
        public async Task<IActionResult> AddResult(ResultDto request)
        {
            var result = new Result
            {
                UserId = request.UserId,
                EventId = request.EventId,
                FinishTime = request.FinishTime,
                Position = request.Position
            };
            _context.Results.Add(result);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Result captured successfully!" });
        }

        [HttpGet("my-results")] // Participant only
        [SessionAuthorize("Participant")]
        public async Task<IActionResult> GetMyResults()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var results = await _context.Results.Where(r => r.UserId == userId).ToListAsync();
            return Ok(results);
        }
    }
}