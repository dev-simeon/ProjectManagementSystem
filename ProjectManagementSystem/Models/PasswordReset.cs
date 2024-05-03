namespace ProjectManagementSystem.Models
{
    public class PasswordReset
    {
        public PasswordReset(string userId, Guid code)
        {
            UserId = userId;
            ResetCode = code;
        }

        public string UserId { get; set; }
        public Guid ResetCode { get; set; }
        public DateTime DateCreated { get;} = DateTime.UtcNow;
        public DateTime ExpiryDate { get; } = DateTime.UtcNow.AddMinutes(30);

    }
}
