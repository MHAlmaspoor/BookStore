using System.Reflection.Metadata.Ecma335;
using BookStore.ProductService.Application.Products.Command.ChangeProductPrice;
using BookStore.ProductService.Application.Products.Command.CreateProduct;
using BookStore.ProductService.Application.Products.Command.DeleteProduct;
using BookStore.ProductService.Application.Products.Command.GetProduct;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BookStore.ProductService.Api.Controllers;

[ApiController]
[Route("api/products")]

public class ProductsController:ControllerBase
{
    private readonly ISender _sender;

    private readonly HealthCheckService _healthCheckService;
    public ProductsController(ISender sender)
    {
        _sender=sender;
    }


    [HttpPost]
    //[Authorize(Policy = "permission:product:create")]
    public async Task<ActionResult<Guid>> Create(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var id=await _sender.Send(command,cancellationToken);

        return CreatedAtAction(nameof(GetById),new{id},id);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetProductQuery(id), cancellationToken);

        if(result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPut("{id:guid}/price")]
    public async Task<IActionResult> ChangePrice(Guid id,[FromBody]ChangeProductPriceCommand request, CancellationToken cancellationToken)
    {
        await _sender.Send( new ChangeProductPriceCommand(id, request.Price, request.Currency), cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteProductCommand(id), cancellationToken);

        return NoContent();
    }



}
