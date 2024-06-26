namespace LargeDataRetrievalAPI.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Task> Tasks { get; set; }
    }

}
