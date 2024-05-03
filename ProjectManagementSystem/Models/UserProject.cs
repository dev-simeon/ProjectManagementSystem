using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models
{
    public class UserProject
    {
        private UserProject() { }

        public UserProject(Project project, User user, CollaboratorRoles role)
        {
            ArgumentNullException.ThrowIfNull(project, nameof(project));
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            ArgumentNullException.ThrowIfNull(role, nameof(role));

            UserId = user.Id;
            ProjectId = project.Id;
            Role = role;
            CreatedAt = DateTime.UtcNow;
        }

        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; private set; }

        [Required(ErrorMessage = "ProjectId is required")]
        public int ProjectId { get; private set; }

        [Required(ErrorMessage = "Role is required")]
        public CollaboratorRoles Role { get; private set; }
        public DateTime CreatedAt { get; private set; }
    }

    public enum CollaboratorRoles
    {
        OWNER,
        PM,
        UI_UX,
        DEVELOPER
    }
}

