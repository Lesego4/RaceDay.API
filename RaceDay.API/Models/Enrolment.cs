namespace RaceDay.API.Models
{
    public class Enrolment
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Link to Participant
        public int EventId { get; set; } // Link to Event
        public int CategoryId { get; set; } // Link to Category
    }
}