using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models
{
    public class Task
    {
        private Task() { }

        public Task(Feature feature, string title, DateTime dueDate, Priority priority)
        {
            ArgumentNullException.ThrowIfNull(feature, nameof(feature));
            ArgumentException.ThrowIfNullOrWhiteSpace(title, nameof(title));

            FeatureId = feature.Id;
            Title = title;
            DueDate = dueDate;
            Status = Status.NOT_STARTED;
            Priority = priority;
            DateCreatedUtc = DateTime.UtcNow;
        }

        public void AssignToUser(User user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            AssigneeId = user.Id;
            AssignedDate = DateTime.UtcNow;
        }

        public void UpdateDueDate(DateTime newDueDate)
        {
            if (newDueDate < DateTime.UtcNow)
            {
                throw new ArgumentException("Due date cannot be in the past.");
            }

            DueDate = newDueDate;
        }

        public void UpdatePriority(Priority newPriority)
        {
            Priority = newPriority;
        }

        public void UpdateStatus(Status newStatus)
        {
            Status = newStatus;
        }

        public void AddOrUpdateNote(string note)
        {
            Note = note;
        }

        public int Id { get; private set; }

        [Required(ErrorMessage = "FeatureId is required")]
        public int FeatureId { get; private set; }

        [Required(ErrorMessage = "Task title is required")]
        public string Title { get; private set; }

        public string? Note { get; private set; }

        public DateTime DateCreatedUtc { get; private set; }

        public DateTime? AssignedDate { get; private set; }

        public int? AssigneeId { get; private set; }

        [Required(ErrorMessage = "DueDate iss required")]
        public DateTime DueDate { get; private set; }

        public Status Status { get; private set; }

        public Priority Priority { get; private set; }

        public User Assignee { get; private set; }
        public Feature Feature { get; private set; }
    }

    public enum Priority
    {
        NOT_IMPORTANT,
        IMPORTANT
    }

    public enum Status
    {
        COMPLETED,
        ONGOING,
        NOT_STARTED
    }
}
