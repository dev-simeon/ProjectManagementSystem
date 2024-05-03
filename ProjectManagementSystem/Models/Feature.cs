
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models
{
    public class Feature
    {
        public Feature()
        {}

        public Feature(Project project, string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
            ArgumentNullException.ThrowIfNull(project, nameof(project));

            ProjectId = project.Id;
            Name = name;
            CreatedAt = DateTime.UtcNow;
        }

        public int Id { get; private set; }
        [Required(ErrorMessage = "ProjectId is required")]
        public int ProjectId { get; private set; }
        [Required(ErrorMessage = "Feature name is required")]
        public string Name { get; private set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; private set; }

        public Project Project { get; private set; }
        public ICollection<Task> Tasks { get; private set; } = [];
    }
}
