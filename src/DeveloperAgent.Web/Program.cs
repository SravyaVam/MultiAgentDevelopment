using DeveloperAgent.Application.UseCases;
using DeveloperAgent.Domain.Interfaces;
using DeveloperAgent.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// TODO: Replace with real auth & configuration for production
builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
{
    // For demo only: leave options for configuration with real authority in production
});

// CORS: keep restrictive in production; for the sample, configure a named policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.WithOrigins("https://localhost:5001")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

// DI registrations
builder.Services.AddSingleton<ITodoRepository, InMemoryTodoRepository>();
builder.Services.AddScoped<GetTodosUseCase>();

// User management repository & usecases
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddScoped<GetUsersUseCase>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("DefaultPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
