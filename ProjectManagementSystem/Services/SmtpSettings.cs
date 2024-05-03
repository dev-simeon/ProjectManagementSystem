namespace ProjectManagementSystem.Services
{
    public class SmtpSettings
    {
        public string ServerEndpoint { get; init; }
        public string DisplayName { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
        public bool UseSSL { get; init; }
    }

}
