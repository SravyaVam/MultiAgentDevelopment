using DeveloperAgent.Application.DTOs;
using DeveloperAgent.Application.UseCases;
using DeveloperAgent.Domain.Interfaces;
using DeveloperAgent.Web.Controllers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading;
using Xunit;

namespace DeveloperAgent.Web.Tests
{
    public class TodosControllerTests
    {
        [Fact]
        public async Task Get_Returns_Items()
        {
            var domainItems = new List<DeveloperAgent.Domain.Entities.TodoItem>
            {
                new DeveloperAgent.Domain.Entities.TodoItem("T1") { Id = 1, IsComplete = false }
            };

            var repoMock = new Mock<ITodoRepository>();
            repoMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(domainItems as IEnumerable<DeveloperAgent.Domain.Entities.TodoItem>);
            var useCase = new GetTodosUseCase(repoMock.Object);
            var loggerMock = new Mock<ILogger<TodosController>>();

            var controller = new TodosController(useCase, repoMock.Object, loggerMock.Object);
            var result = await controller.Get(CancellationToken.None);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsAssignableFrom<IEnumerable<TodoItemDto>>(ok.Value);
            Assert.Single(returned);
        }

        [Fact]
        public async Task Create_Returns_Created()
        {
            var request = new DeveloperAgent.Web.Models.CreateTodoRequest { Title = "New task", IsComplete = false };

            var useCase = new GetTodosUseCase(Mock.Of<ITodoRepository>());
            var repoMock = new Mock<ITodoRepository>();
            repoMock.Setup(x => x.AddAsync(It.IsAny<DeveloperAgent.Domain.Entities.TodoItem>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DeveloperAgent.Domain.Entities.TodoItem ti, CancellationToken ct) => { ti.Id = 123; return ti; });
            repoMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var loggerMock = new Mock<ILogger<TodosController>>();
            var controller = new TodosController(useCase, repoMock.Object, loggerMock.Object);

            var result = await controller.Create(request, CancellationToken.None);
            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            var dto = Assert.IsType<TodoItemDto>(created.Value);
            Assert.Equal(123, dto.Id);
        }

        [Fact]
        public async Task Create_With_InvalidModel_Returns_BadRequest()
        {
            var request = new DeveloperAgent.Web.Models.CreateTodoRequest { Title = "" };
            var useCase2 = new GetTodosUseCase(Mock.Of<ITodoRepository>());
            var repoMock = new Mock<ITodoRepository>();
            var loggerMock = new Mock<ILogger<TodosController>>();

            var controller = new TodosController(useCase2, repoMock.Object, loggerMock.Object);
            controller.ModelState.AddModelError("Title", "Required");

            var result = await controller.Create(request, CancellationToken.None);
            Assert.IsType<ObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetById_InvalidId_Returns_BadRequest()
        {
            var useCase = new GetTodosUseCase(Mock.Of<ITodoRepository>());
            var repoMock = new Mock<ITodoRepository>();
            var loggerMock = new Mock<ILogger<TodosController>>();

            var controller = new TodosController(useCase, repoMock.Object, loggerMock.Object);

            var result = await controller.GetById(0, CancellationToken.None);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetById_NotFound_Returns_NotFound()
        {
            var useCase = new GetTodosUseCase(Mock.Of<ITodoRepository>());
            var repoMock = new Mock<ITodoRepository>();
            repoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DeveloperAgent.Domain.Entities.TodoItem?)null);
            var loggerMock = new Mock<ILogger<TodosController>>();

            var controller = new TodosController(useCase, repoMock.Object, loggerMock.Object);

            var result = await controller.GetById(123, CancellationToken.None);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetById_Returns_Item()
        {
            var item = new DeveloperAgent.Domain.Entities.TodoItem("T1") { Id = 5, IsComplete = true };
            var useCase = new GetTodosUseCase(Mock.Of<ITodoRepository>());
            var repoMock = new Mock<ITodoRepository>();
            repoMock.Setup(x => x.GetByIdAsync(5, It.IsAny<CancellationToken>()))
                .ReturnsAsync(item);
            var loggerMock = new Mock<ILogger<TodosController>>();

            var controller = new TodosController(useCase, repoMock.Object, loggerMock.Object);
            var result = await controller.GetById(5, CancellationToken.None);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var dto = Assert.IsType<TodoItemDto>(ok.Value);
            Assert.Equal(5, dto.Id);
            Assert.Equal("T1", dto.Title);
            Assert.True(dto.IsComplete);
        }

        [Fact]
        public async Task Update_InvalidId_Returns_BadRequest()
        {
            var useCase = new GetTodosUseCase(Mock.Of<ITodoRepository>());
            var repoMock = new Mock<ITodoRepository>();
            var loggerMock = new Mock<ILogger<TodosController>>();

            var controller = new TodosController(useCase, repoMock.Object, loggerMock.Object);
            var request = new DeveloperAgent.Web.Models.UpdateTodoRequest { Title = "x", IsComplete = true };

            var result = await controller.Update(0, request, CancellationToken.None);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Update_InvalidModel_Returns_ValidationProblem()
        {
            var useCase = new GetTodosUseCase(Mock.Of<ITodoRepository>());
            var repoMock = new Mock<ITodoRepository>();
            var loggerMock = new Mock<ILogger<TodosController>>();
            var controller = new TodosController(useCase, repoMock.Object, loggerMock.Object);
            var request = new DeveloperAgent.Web.Models.UpdateTodoRequest { Title = "" };
            controller.ModelState.AddModelError("Title", "Required");

            var result = await controller.Update(1, request, CancellationToken.None);
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task Update_NotFound_Returns_NotFound()
        {
            var useCase = new GetTodosUseCase(Mock.Of<ITodoRepository>());
            var repoMock = new Mock<ITodoRepository>();
            repoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((DeveloperAgent.Domain.Entities.TodoItem?)null);
            var loggerMock = new Mock<ILogger<TodosController>>();
            var controller = new TodosController(useCase, repoMock.Object, loggerMock.Object);
            var request = new DeveloperAgent.Web.Models.UpdateTodoRequest { Title = "x", IsComplete = false };

            var result = await controller.Update(1, request, CancellationToken.None);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_Returns_NoContent()
        {
            var useCase = new GetTodosUseCase(Mock.Of<ITodoRepository>());
            var repoMock = new Mock<ITodoRepository>();
            var existing = new DeveloperAgent.Domain.Entities.TodoItem("Old") { Id = 2, IsComplete = false };
            repoMock.Setup(x => x.GetByIdAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(existing);
            repoMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);
            var loggerMock = new Mock<ILogger<TodosController>>();
            var controller = new TodosController(useCase, repoMock.Object, loggerMock.Object);
            var request = new DeveloperAgent.Web.Models.UpdateTodoRequest { Title = "New", IsComplete = true };

            var result = await controller.Update(2, request, CancellationToken.None);
            Assert.IsType<NoContentResult>(result);
            Assert.Equal("New", existing.Title);
            Assert.True(existing.IsComplete);
        }

        [Fact]
        public async Task Delete_InvalidId_Returns_BadRequest()
        {
            var useCase = new GetTodosUseCase(Mock.Of<ITodoRepository>());
            var repoMock = new Mock<ITodoRepository>();
            var loggerMock = new Mock<ILogger<TodosController>>();
            var controller = new TodosController(useCase, repoMock.Object, loggerMock.Object);

            var result = await controller.Delete(0, CancellationToken.None);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_NotFound_When_GetById_Returns_NotFound()
        {
            var useCase = new GetTodosUseCase(Mock.Of<ITodoRepository>());
            var repoMock = new Mock<ITodoRepository>();
            repoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((DeveloperAgent.Domain.Entities.TodoItem?)null);
            var loggerMock = new Mock<ILogger<TodosController>>();
            var controller = new TodosController(useCase, repoMock.Object, loggerMock.Object);

            var result = await controller.Delete(5, CancellationToken.None);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Returns_NoContent_When_Deleted()
        {
            var useCase = new GetTodosUseCase(Mock.Of<ITodoRepository>());
            var repoMock = new Mock<ITodoRepository>();
            var existing = new DeveloperAgent.Domain.Entities.TodoItem("T") { Id = 10 };
            repoMock.Setup(x => x.GetByIdAsync(10, It.IsAny<CancellationToken>())).ReturnsAsync(existing);
            repoMock.Setup(x => x.DeleteAsync(10, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            repoMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);
            var loggerMock = new Mock<ILogger<TodosController>>();
            var controller = new TodosController(useCase, repoMock.Object, loggerMock.Object);

            var result = await controller.Delete(10, CancellationToken.None);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_Returns_NotFound_When_DeleteReturnsFalse()
        {
            var useCase = new GetTodosUseCase(Mock.Of<ITodoRepository>());
            var repoMock = new Mock<ITodoRepository>();
            var existing = new DeveloperAgent.Domain.Entities.TodoItem("T") { Id = 10 };
            repoMock.Setup(x => x.GetByIdAsync(10, It.IsAny<CancellationToken>())).ReturnsAsync(existing);
            repoMock.Setup(x => x.DeleteAsync(10, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            var loggerMock = new Mock<ILogger<TodosController>>();
            var controller = new TodosController(useCase, repoMock.Object, loggerMock.Object);

            var result = await controller.Delete(10, CancellationToken.None);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
