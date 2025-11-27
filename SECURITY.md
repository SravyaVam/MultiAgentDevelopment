# Security & Controller Best Practices

This repository includes a secure `TodosController` implementation and an in-memory repository for local testing.

Key fixes and best practices applied:

- Use DTOs for input and output to prevent overposting and leaking domain objects.
- Validate input with DataAnnotations and reject invalid requests with proper status codes.
- Introduce `async`/`await` and cancellation support for asynchronous operations.
- Use dependency injection for services and repositories.
- Use `ILogger` for structured logging and error diagnostics; avoid leaking internal error details.
- Add `[ApiController]`, `CreatedAtAction`, `NoContent`, `Ok`, `NotFound`, and `BadRequest` to return appropriate HTTP status codes.
- Add `[Authorize]` attribute to require authentication for most endpoints; `AllowAnonymous` is specified for GET list only for demo purposes (remove in production).
- Configure CORS with a named policy to avoid wide-open CORS settings.
- Configure HTTPS redirection and enable Swagger in Development for API discovery.

Notes & TODOs:
- Authentication is stubbed for demo; configure a real JWT provider or Identity provider in production and avoid anonymous access if not intended.
- `InMemoryTodoRepository` is provided for test and demo purposes — use a robust database-backed repository for production.
- Implement transaction and concurrency control in repository layer.

If you'd like, I can also:
- Add a real authentication and authorization example using JwtBearer.
- Add role-based authorization to endpoints.
- Add integration tests that run against an in-memory TestServer.
