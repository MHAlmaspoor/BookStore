using BookStore.ProductService.Application.Products.Command.CreateProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.ProductService.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController:ControllerBase
{
    private readonly ISender _sender;
    public ProductsController(ISender sender)
    {
        _sender=sender;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var id=await _sender.Send(command,cancellationToken);

        return CreatedAtAction(nameof(GetById),new{id},id);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        return Ok();
    }
}
