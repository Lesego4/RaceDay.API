using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Filters;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CategoriesController(AppDbContext context) { _context = context; }

        [HttpGet("event/{eventId}")] // Both roles can view
        public async Task<IActionResult> GetCategoriesForEvent(int eventId)
        {
            var categories = await _context.Categories.Where(c => c.EventId == eventId).ToListAsync();
            return Ok(categories);
        }

        [HttpPost] // Organiser only
        [SessionAuthorize("Organiser")]
        public async Task<IActionResult> CreateCategory(Category request)
        {
            _context.Categories.Add(request);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Category created successfully!" });
        }
    }
}