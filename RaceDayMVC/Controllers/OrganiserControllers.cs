using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models;
using RaceDayMVC.Services;

namespace RaceDayMVC.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly ApiService _apiService;

        public OrganiserController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Dashboard()
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }

            var events = await _apiService.GetEventsAsync();
            return View(events);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
                return RedirectToAction("Login", "Account");

            var success = await _apiService.DeleteEventAsync(id);

            if (success)
            {
                return RedirectToAction("Dashboard");
            }

            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
                return RedirectToAction("Login", "Account");

            var ev = await _apiService.GetEventByIdAsync(id);
            if (ev == null) return NotFound();

            return View(ev);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EventDto updatedEvent)
        {
            if (!ModelState.IsValid)
                return View(updatedEvent);

            var success = await _apiService.UpdateEventAsync(updatedEvent);
            if (success)
                return RedirectToAction("Dashboard");

            ViewBag.Error = "Failed to update the event.";
            return View(updatedEvent);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("UserRole") != "Organiser")
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EventDto newEvent, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
                return View(newEvent);

            var error = await _apiService.CreateEventAsync(newEvent, imageFile);

            if (error == null)
                return RedirectToAction("Dashboard");

            ViewBag.Error = $"API Error: {error}";
            return View(newEvent);
        }
    }
}