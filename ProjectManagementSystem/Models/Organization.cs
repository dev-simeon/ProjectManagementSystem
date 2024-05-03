//namespace ProjectManagementSystem.Models
//{
//    public class Organization
//    {
//        public Organization(string organizationName, string adminFirstName, string adminLastName, string adminEmail, string adminMobile)
//        {
//            var user = new User(adminFirstName, adminLastName, adminEmail, adminMobile, this);
//            Name = organizationName;
//            CreatedAt = DateTime.UtcNow;

//            Users.Add(user);
//        }
//        public int Id { get; private set; }
//        public string Name { get; private set; }
//        public DateTime CreatedAt { get; private set; }


//        public ICollection<Project> Projects { get; private set; }
//        public ICollection<User> Users { get; private set; }
//    }
//}
