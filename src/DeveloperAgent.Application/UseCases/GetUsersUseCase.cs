using DeveloperAgent.Application.DTOs;
using DeveloperAgent.Domain.Interfaces;

namespace DeveloperAgent.Application.UseCases
{
    public class GetUsersUseCase
    {
        private readonly IUserRepository _repository;
        public GetUsersUseCase(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserDto>> ExecuteAsync(CancellationToken ct = default)
        {
            var users = await _repository.GetAllAsync(ct);
            return users.Select(x => new UserDto
            {
                Id = x.Id,
                Username = x.Username,
                Email = x.Email,
                DisplayName = x.DisplayName,
                IsActive = x.IsActive,
                LastLoginAtUtc = x.LastLoginAtUtc
            });
        }
    }
}
