using DeveloperAgent.Domain.Entities;

namespace DeveloperAgent.Domain.Interfaces
{
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoItem>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TodoItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<TodoItem> AddAsync(TodoItem item, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}