namespace RaceDay.API.Models
{
    public class EnrolmentDto
    {
        public int EventId { get; set; }
        public int CategoryId { get; set; }
    }

    public class ResultDto
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public string FinishTime { get; set; } = string.Empty;
        public int Position { get; set; }
    }
}