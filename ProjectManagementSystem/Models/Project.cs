using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models
{
    public class Project
    {
        private Project() { }

        /// <summary>
        /// Creates a new project for a user.
        /// </summary>
        /// <param name="user">the owner of this project.</param>
        /// <param name="name">the name of the project</param>
        /// <param name="description">a brief description about the project.</param>
        /// <param name="startDate">the date the project starts</param>
        /// <param name="endDate">the date the project ends</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Project(User user, string name, string description, DateTime startDate, DateTime endDate)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
            ArgumentException.ThrowIfNullOrWhiteSpace(description, nameof(description));

            if (endDate <= startDate)
            {
                throw new ArgumentOutOfRangeException(nameof(endDate), "End date should be greater than start date.");
            }

            if (startDate.ToUniversalTime() < DateTime.UtcNow.Date)
            {
                throw new ArgumentOutOfRangeException(nameof(startDate), "Start date should be at least 3 minutes from current time");
            }

            Name = name;
            Description = description;
            DateCreatedUtc = DateTime.UtcNow;
            StartDate = startDate;
            EndDate = endDate;
            MakeOwner(user);
        }


        //private static string GenerateUniqueId(string name)
        //{
        //    // Convert name to bytes
        //    byte[] nameBytes = Encoding.UTF8.GetBytes(name);

        //    // Compute hash
        //    byte[] hashBytes = SHA256.HashData(nameBytes);

        //    // Convert hash to string
        //    StringBuilder builder = new();
        //    foreach (byte b in hashBytes)
        //    {
        //        builder.Append(b.ToString("x2")); // Convert byte to hexadecimal string
        //    }
        //    return builder.ToString();
        //}

        public void MakeOwner(User owner)
        {
            var ownerCollaborator = new UserProject(this, owner, CollaboratorRoles.OWNER);
            Collaborators.Add(ownerCollaborator);
        }

        public void AddCollaborator(User collaborator, CollaboratorRoles role)
        {
            var collaboratorProject = new UserProject(this, collaborator, role);
            Collaborators.Add(collaboratorProject);
        }

        public int Id { get; private set; }

        [Required(ErrorMessage = "Project name is required")]
        public string Name { get; private set; }

        [Required(ErrorMessage = "Project description is required")]
        public string Description { get; private set; }

        public DateTime DateCreatedUtc { get; private set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; private set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; private set; }

        public ICollection<User> Users { get; private set; } = [];
        public ICollection<UserProject> Collaborators { get; private set; } = [];
        public ICollection<Feature> Features { get; private set; } = [];
    }
}
