using LargeDataRetrievalAPI.Enums;

namespace LargeDataRetrievalAPI.Models
{
    public class Task
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public long AssignedToUserId { get; set; }
        public User AssignedToUser { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
    }
}
