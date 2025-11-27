using DeveloperAgent.Application.DTOs;
using DeveloperAgent.Application.UseCases;
using DeveloperAgent.Domain.Entities;
using DeveloperAgent.Domain.Interfaces;
using DeveloperAgent.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperAgent.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // For demo; attach a real authentication scheme in production
    public class TodosController : ControllerBase
    {
        private readonly GetTodosUseCase _getTodos;
        private readonly ITodoRepository _repository;
        private readonly ILogger<TodosController> _logger;

        public TodosController(GetTodosUseCase getTodos, ITodoRepository repository, ILogger<TodosController> logger)
        {
            _getTodos = getTodos;
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous] // Allow listing without auth for demo; remove in production
        public async Task<ActionResult<IEnumerable<TodoItemDto>>> Get(CancellationToken ct)
        {
            try
            {
                var items = await _getTodos.ExecuteAsync(ct);
                return Ok(items);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                _logger.LogInformation("Request cancelled");
                return StatusCode(StatusCodes.Status499ClientClosedRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Todos");
                return Problem("An unexpected error occurred.");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TodoItemDto>> GetById(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be greater than 0.");
            }

            var item = await _repository.GetByIdAsync(id, ct);
            if (item == null)
                return NotFound();

            var dto = new TodoItemDto { Id = item.Id, Title = item.Title, IsComplete = item.IsComplete };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemDto>> Create([FromBody] CreateTodoRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var todo = new TodoItem(request.Title) { IsComplete = request.IsComplete };

            try
            {
                var created = await _repository.AddAsync(todo, ct);
                await _repository.SaveChangesAsync(ct);
                var dto = new TodoItemDto { Id = created.Id, Title = created.Title, IsComplete = created.IsComplete };
                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Todo");
                return Problem("An error occurred while creating the todo item.");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTodoRequest request, CancellationToken ct)
        {
            if (id <= 0)
                return BadRequest("Id must be greater than 0.");

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var existing = await _repository.GetByIdAsync(id, ct);
            if (existing == null)
                return NotFound();

            existing.Title = request.Title;
            existing.IsComplete = request.IsComplete;

            try
            {
                await _repository.SaveChangesAsync(ct);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating todo");
                return Problem("An error occurred while updating the todo item.");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            if (id <= 0)
                return BadRequest("Id must be greater than 0.");

            var existing = await _repository.GetByIdAsync(id, ct);
            if (existing == null)
                return NotFound();

            try
            {
                var deleted = await _repository.DeleteAsync(id, ct);
                if (!deleted)
                    return NotFound();

                await _repository.SaveChangesAsync(ct);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting todo");
                return Problem("An error occurred while deleting the todo item.");
            }
        }
    }
}
