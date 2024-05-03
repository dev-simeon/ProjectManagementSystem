using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.ViewModel
{
    public class CollaboratorViewModel
    {
        public CollaboratorViewModel(string fullName, CollaboratorRoles role)
        {
            FullName = fullName;
            Role = role.ToString();
        }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}
