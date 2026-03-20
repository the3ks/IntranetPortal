namespace IntranetPortal.Data.Models
{
    public class Announcement
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public int AuthorId { get; set; }
        public Employee? Author { get; set; }
    }
}
