using DeveloperAgent.Domain.Entities;
using DeveloperAgent.Domain.Interfaces;

namespace DeveloperAgent.Infrastructure.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = new();
    private readonly List<Role> _roles = new();
    private readonly List<UserRole> _userRoles = new();
    private readonly List<RefreshToken> _refreshTokens = new();
    private readonly List<AuditTrail> _auditTrails = new();
    private int _nextUserId = 1;
    private int _nextRoleId = 1;
    private int _nextTokenId = 1;

    public InMemoryUserRepository()
    {
        // Seed a default admin user and role
        var adminRole = new Role { Id = _nextRoleId++, Name = "Administrator", Description = "System admin" };
        _roles.Add(adminRole);

        var admin = new User { Id = _nextUserId++, Username = "admin", Email = "admin@example.com", PasswordHash = "<seeded>" };
        _users.Add(admin);
        _userRoles.Add(new UserRole { UserId = admin.Id, RoleId = adminRole.Id, User = admin, Role = adminRole });
    }

    public Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        user.Id = _nextUserId++;
        user.CreatedAtUtc = DateTime.UtcNow;
        _users.Add(user);
        return Task.FromResult(user);
    }

    public Task AssignRoleAsync(int userId, int roleId, CancellationToken cancellationToken = default)
    {
        if (!_userRoles.Any(x => x.UserId == userId && x.RoleId == roleId))
        {
            var user = _users.SingleOrDefault(u => u.Id == userId);
            var role = _roles.SingleOrDefault(r => r.Id == roleId);
            var ur = new UserRole { UserId = userId, RoleId = roleId, User = user, Role = role };
            _userRoles.Add(ur);
        }
        return Task.CompletedTask;
    }

    public Task AddAuditTrailAsync(AuditTrail entry, CancellationToken cancellationToken = default)
    {
        entry.Id = _auditTrails.Count > 0 ? _auditTrails.Max(a => a.Id) + 1 : 1;
        entry.TimestampUtc = entry.TimestampUtc == default ? DateTime.UtcNow : entry.TimestampUtc;
        _auditTrails.Add(entry);
        return Task.CompletedTask;
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = _users.SingleOrDefault(x => x.Id == id);
        if (user == null) return Task.FromResult(false);
        _users.Remove(user);
        _userRoles.RemoveAll(ur => ur.UserId == id);
        _refreshTokens.RemoveAll(rt => rt.UserId == id);
        return Task.FromResult(true);
    }

    public Task<IEnumerable<Role>> GetAllRolesAsync(CancellationToken cancellationToken = default) => Task.FromResult<IEnumerable<Role>>(_roles);

    public Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default) => Task.FromResult<IEnumerable<User>>(_users);

    public Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default) => Task.FromResult(_users.SingleOrDefault(x => x.Id == id));

    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default) => Task.FromResult(_users.SingleOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)));

    public Task<Role?> GetRoleByNameAsync(string name, CancellationToken cancellationToken = default) => Task.FromResult(_roles.SingleOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)));

    public Task<Role> AddRoleAsync(Role role, CancellationToken cancellationToken = default)
    {
        role.Id = _nextRoleId++;
        _roles.Add(role);
        return Task.FromResult(role);
    }

    public Task<bool> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var existing = _users.SingleOrDefault(x => x.Id == user.Id);
        if (existing == null) return Task.FromResult(false);
        existing.Username = user.Username;
        existing.Email = user.Email;
        existing.DisplayName = user.DisplayName;
        existing.IsActive = user.IsActive;
        existing.UpdatedAtUtc = DateTime.UtcNow;
        return Task.FromResult(true);
    }

    public Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default) => Task.FromResult(true);

    public Task<IEnumerable<AuditTrail>> GetAuditEntriesForEntityAsync(string entityType, string? entityId = null, CancellationToken cancellationToken = default)
    {
        var query = _auditTrails.AsEnumerable();
        query = query.Where(a => a.EntityType == entityType);
        if (entityId != null) query = query.Where(a => a.EntityId == entityId);
        return Task.FromResult<IEnumerable<AuditTrail>>(query);
    }
}
