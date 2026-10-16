namespace RaceDay.API.Models
{
    public class Result
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public string FinishTime { get; set; } = string.Empty; // e.g., "02:15:30"
        public int Position { get; set; }
    }
}