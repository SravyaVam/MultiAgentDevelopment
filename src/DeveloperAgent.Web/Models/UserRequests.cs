namespace DeveloperAgent.Web.Models
{
    public class CreateUserRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // In a real app, use a secure mechanism and never transport plain passwords without TLS
        public string? DisplayName { get; set; }
    }

    public class UpdateUserRequest
    {
        public string? DisplayName { get; set; }
        public bool? IsActive { get; set; }
    }
}
