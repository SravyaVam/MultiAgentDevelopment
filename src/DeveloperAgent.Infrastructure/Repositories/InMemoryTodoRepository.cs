using DeveloperAgent.Domain.Entities;
using DeveloperAgent.Domain.Interfaces;

namespace DeveloperAgent.Infrastructure.Repositories;

public class InMemoryTodoRepository : ITodoRepository
{
    private readonly List<TodoItem> _items = new()
    {
        new TodoItem("Buy milk") { Id = 1, IsComplete = false },
        new TodoItem("Write report") { Id = 2, IsComplete = true },
    };
    private int _nextId = 3;

    public Task<IEnumerable<TodoItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<TodoItem>>(_items);
    }

    public Task<TodoItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = _items.SingleOrDefault(x => x.Id == id);
        return Task.FromResult(item);
    }

    public Task<TodoItem> AddAsync(TodoItem item, CancellationToken cancellationToken = default)
    {
        item.Id = _nextId++;
        _items.Add(item);
        return Task.FromResult(item);
    }

    public Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // No-op for in-memory, return true
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = _items.SingleOrDefault(x => x.Id == id);
        if (item == null)
            return Task.FromResult(false);

        _items.Remove(item);
        return Task.FromResult(true);
    }
}
