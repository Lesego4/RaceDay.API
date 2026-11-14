using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models;
using RaceDayMVC.Services;

namespace RaceDayMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService _apiService;

        public AccountController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            // Call the API
            var authResponse = await _apiService.LoginAsync(request);

            if (authResponse != null)
            {
                // Success! Save the user's role and ID in the session
                // Safe way to save to session (prevents crashing if data is null)
                HttpContext.Session.SetString("UserRole", authResponse.Role ?? "Participant");

                if (!string.IsNullOrEmpty(authResponse.UserId))
                {
                    HttpContext.Session.SetString("UserId", authResponse.UserId);
                }

                // Redirect based on their role
                if (authResponse.Role == "Organiser")
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            // If we get here, login failed
            ViewBag.ErrorMessage = "Invalid email or password.";
            return View(request);
        }
    }
}