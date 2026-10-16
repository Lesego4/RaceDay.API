namespace RaceDay.API.Models
{
    public class Category
    {
        public int Id { get; set; }
        public int EventId { get; set; } // Link to Event
        public string Name { get; set; } = string.Empty; // e.g., "Under 20", "10km"
    }
}