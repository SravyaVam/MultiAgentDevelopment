namespace DeveloperAgent.Domain.Entities
{
    // Join entity for Users and Roles (many-to-many)
    public class UserRole
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
