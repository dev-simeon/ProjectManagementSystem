namespace ProjectManagementSystem.Models
{
    public class Notification
    {
        public Notification()
        {}

        public Notification(User user, string content, DateTime createdAt, bool isRead)
        {
            UserId = user.Id;
            Content = content;
            CreatedAt = createdAt;
            IsRead = isRead;
        }

        public int Id { get; private set; }
        public int UserId { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsRead { get; private set; }

        public User User { get; private set; }
    }
}
