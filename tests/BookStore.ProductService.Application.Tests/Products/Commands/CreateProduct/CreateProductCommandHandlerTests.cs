using BookStore.ProductService.Application.Abstraction.Persistance;
using BookStore.ProductService.Application.Products.Command.CreateProduct;
using BookStore.ProductService.Domain.Products;
using Moq;

namespace BookStore.ProductService.Application.Tests.Products.Commands.CreateProduct;

public class CreateProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_create_product_And_Save_Changes()
    {
        //Arrange
        var repository=new Mock<IProductRepository>();
        var unitoFWork=new Mock<IUnitOfWork>();

        Product? captureProduct=null;

        repository.Setup(r=>r.AddAsync(
            It.IsAny<Product>(),
            It.IsAny<CancellationToken>()))
            .Callback<Product,CancellationToken>((product,_)=>
            {
                captureProduct=product;
            }).Returns(Task.CompletedTask);

        var handler=new CreateProductCommandHandler(
            repository.Object,unitoFWork.Object);

        var command =new CreateProductCommand(
            "DB Book",
            100,
            "EUR");

        //Act
        var id=await handler.Handle(command,CancellationToken.None);

        //Assert
        Assert.NotEqual(Guid.Empty,id);
        Assert.NotNull(captureProduct);
        Assert.Equal(command.Name, captureProduct!.Name);
        Assert.Equal(command.Price,captureProduct.Price.Amount);
        Assert.Equal(command.Currency,captureProduct.Price.Currency);

        repository.Verify(r=>r.AddAsync(
            It.IsAny<Product>(),
            It.IsAny<CancellationToken>()),Times.Once);

        unitoFWork.Verify(u=>u.SaveChangesAsync(It.IsAny<CancellationToken>()),
        Times.Once);

    }
}
