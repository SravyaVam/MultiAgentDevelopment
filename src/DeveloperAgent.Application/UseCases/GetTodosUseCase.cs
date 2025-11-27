using DeveloperAgent.Application.DTOs;
using DeveloperAgent.Domain.Interfaces;
using DeveloperAgent.Domain.Entities;

namespace DeveloperAgent.Application.UseCases
{
    public class GetTodosUseCase
    {
        private readonly ITodoRepository _repository;
        public GetTodosUseCase(ITodoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TodoItemDto>> ExecuteAsync(CancellationToken ct = default)
        {
            var items = await _repository.GetAllAsync(ct);
            return items.Select(x => new TodoItemDto { Id = x.Id, Title = x.Title, IsComplete = x.IsComplete });
        }
    }
}