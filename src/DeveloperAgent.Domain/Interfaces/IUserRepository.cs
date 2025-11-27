using DeveloperAgent.Domain.Entities;

namespace DeveloperAgent.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
        Task<User> AddAsync(User user, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(User user, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);

        // Roles & tokens
        Task<IEnumerable<Role>> GetAllRolesAsync(CancellationToken cancellationToken = default);
        Task<Role?> GetRoleByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<Role> AddRoleAsync(Role role, CancellationToken cancellationToken = default);
        Task AssignRoleAsync(int userId, int roleId, CancellationToken cancellationToken = default);

        // Audit
        Task AddAuditTrailAsync(AuditTrail entry, CancellationToken cancellationToken = default);
        Task<IEnumerable<AuditTrail>> GetAuditEntriesForEntityAsync(string entityType, string? entityId = null, CancellationToken cancellationToken = default);
    }
}
