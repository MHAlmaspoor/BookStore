using Microsoft.AspNetCore.Mvc;

namespace BookStore.ProductService.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController:ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new[]
        {
            "Book",
            "Ebook",
            "NoteBook"
        });
    }
}
