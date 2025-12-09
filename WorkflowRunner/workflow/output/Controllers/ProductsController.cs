using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MyEcommerceAPI.Models;

namespace MyEcommerceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetProducts()
    {
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 999.99m },
            new Product { Id = 2, Name = "Phone", Price = 599.99m }
        };
        return Ok(products);
    }
}