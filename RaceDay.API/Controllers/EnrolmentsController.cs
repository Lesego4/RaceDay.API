using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Filters;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrolmentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public EnrolmentsController(AppDbContext context) { _context = context; }

        [HttpPost] // Participant only
        [SessionAuthorize("Participant")]
        public async Task<IActionResult> Enrol(EnrolmentDto request)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Unauthorized();

            var enrolment = new Enrolment
            {
                UserId = userId.Value,
                EventId = request.EventId,
                CategoryId = request.CategoryId
            };
            _context.Enrolments.Add(enrolment);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Enrolled successfully!" });
        }

        [HttpGet("my-enrolments")] // Participant only
        [SessionAuthorize("Participant")]
        public async Task<IActionResult> GetMyEnrolments()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var enrolments = await _context.Enrolments.Where(e => e.UserId == userId).ToListAsync();
            return Ok(enrolments);
        }

        [HttpGet("event/{eventId}")] // Organiser only
        [SessionAuthorize("Organiser")]
        public async Task<IActionResult> GetEnrolmentsForEvent(int eventId)
        {
            var enrolments = await _context.Enrolments.Where(e => e.EventId == eventId).ToListAsync();
            return Ok(enrolments);
        }
    }
}