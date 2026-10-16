using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Filters;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/events (Both roles can view)
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _context.Events.ToListAsync();
            return Ok(events);
        }

        // GET: api/events/5 (Both roles can view)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null) return NotFound("Event not found.");
            return Ok(eventItem);
        }

        // POST: api/events (Organiser only)
        [HttpPost]
        [SessionAuthorize("Organiser")] // <--- The magic filter!
        public async Task<IActionResult> CreateEvent(EventDto request)
        {
            var newEvent = new Event
            {
                Name = request.Name,
                Description = request.Description,
                Date = request.Date,
                Location = request.Location,
                Distance = request.Distance,
                EventType = request.EventType
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Event created successfully!", EventId = newEvent.Id });
        }

        // PUT: api/events/5 (Organiser only)
        [HttpPut("{id}")]
        [SessionAuthorize("Organiser")]
        public async Task<IActionResult> UpdateEvent(int id, EventDto request)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null) return NotFound("Event not found.");

            eventItem.Name = request.Name;
            eventItem.Description = request.Description;
            eventItem.Date = request.Date;
            eventItem.Location = request.Location;
            eventItem.Distance = request.Distance;
            eventItem.EventType = request.EventType;

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Event updated successfully!" });
        }

        // DELETE: api/events/5 (Organiser only)
        [HttpDelete("{id}")]
        [SessionAuthorize("Organiser")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null) return NotFound("Event not found.");

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Event deleted successfully!" });
        }
    }
}