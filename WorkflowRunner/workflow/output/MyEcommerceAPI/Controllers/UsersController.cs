using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MyEcommerceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = new List<User>
        {
            new User { Id = 1, Name = "John Doe", Email = "john@example.com" },
            new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com" }
        };
        return Ok(users);
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] User user)
    {
        if (user == null) return BadRequest("Invalid user data");
        user.Id = new Random().Next(1000, 9999);
        return Ok(user);
    }

    [HttpGet("{id}")]
    public IActionResult GetUser(int id)
    {
        var user = new User { Id = id, Name = "Sample User", Email = $"user{id}@example.com" };
        return Ok(user);
    }
}