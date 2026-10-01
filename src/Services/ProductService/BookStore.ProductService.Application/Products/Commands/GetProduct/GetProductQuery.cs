using BookStore.ProductService.Domain.Products;
using MediatR;

namespace BookStore.ProductService.Application.Products.Command.GetProduct;

public sealed record GetProductQuery(Guid productId) : IRequest<ProductResponse?>;
