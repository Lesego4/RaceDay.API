using System.Net.Http.Json;
using RaceDayMVC.Models;

namespace RaceDayMVC.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(config["ApiSettings:BaseUrl"]);
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Auth/login", request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<AuthResponse>();

            return null; // Login failed
        }

        public async Task<List<EventDto>> GetEventsAsync()
        {
            var response = await _httpClient.GetAsync("/api/Events");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<EventDto>>();
            }
            return new List<EventDto>();
        }

        // THIS IS THE NEW METHOD WE JUST FIXED
        public async Task<string> CreateEventAsync(EventDto newEvent, IFormFile imageFile)
        {
            using var content = new MultipartFormDataContent();

            // Add the text fields
            content.Add(new StringContent(newEvent.Name ?? ""), "Name");
            content.Add(new StringContent(newEvent.Description ?? ""), "Description");
            content.Add(new StringContent(newEvent.Date.ToString("yyyy-MM-dd")), "Date");
            content.Add(new StringContent(newEvent.Location ?? ""), "Location");
            content.Add(new StringContent(newEvent.Distance.ToString()), "Distance");
            content.Add(new StringContent(newEvent.EventType ?? ""), "EventType");

            // Add the file if it exists
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileContent = new StreamContent(imageFile.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(imageFile.ContentType);
                content.Add(fileContent, "imageFile", imageFile.FileName);
            }

            var response = await _httpClient.PostAsync("/api/Events", content);

            if (response.IsSuccessStatusCode)
                return null; // Success

            return await response.Content.ReadAsStringAsync(); // Return error message
        }
        public async Task<bool> DeleteEventAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Events/{id}");
            return response.IsSuccessStatusCode;
        }
        public async Task<EventDto> GetEventByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Events/{id}");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<EventDto>();
            return null;
        }

        public async Task<bool> UpdateEventAsync(EventDto updatedEvent)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/Events/{updatedEvent.Id}", updatedEvent);
            return response.IsSuccessStatusCode;
        }
    }
}