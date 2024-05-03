using System.ComponentModel.DataAnnotations;
using BCryptNet = BCrypt.Net.BCrypt;

namespace ProjectManagementSystem.Models
{
    public class User
    {
        private User()
        { }

        public User(string firstName, string lastName, string email, string mobile, string password)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName, nameof(firstName));
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName, nameof(lastName));
            ArgumentException.ThrowIfNullOrWhiteSpace(email, nameof(email));
            ArgumentException.ThrowIfNullOrWhiteSpace(mobile, nameof(mobile));
            ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));

            if(email == "omom@gmail.com")
            {
                Id = 1;
            }
            if (email == "john.doe@example.com")
            {
                Id = 2;
            }
            if (email == "jane.smith@example.com")
            {
                Id = 3;
            }

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Mobile = mobile;
            CreatedAt = DateTime.UtcNow;
            Password = HashPassword(password);
        }

        private static string HashPassword(string password)
        {
            return BCryptNet.HashPassword(password);
        }

        public bool VerifyPassword(string password)
        {
            return BCryptNet.Verify(password, Password);
        }

        public void UpdatePassword(string oldPassword, string newPassword)
        {
            if (!VerifyPassword(oldPassword))
            {
                throw new Exception("Invalid old password.");
            }

            Password = HashPassword(newPassword);
        }

        public int Id { get; private set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; private set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; private set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; private set; }

        [Required(ErrorMessage = "Mobile Number is required")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number")]
        public string Mobile { get; private set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [DataType(DataType.Password)]
        public string Password { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public string FullName => $"{FirstName} {LastName}";


        public ICollection<Project> Projects { get; private set; } = [];
        public ICollection<UserProject> UserProjects { get; private set; } = [];
    }
}
