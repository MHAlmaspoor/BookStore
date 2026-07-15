using BookStore.ProductService.Domain.Common;
using BookStore.ProductService.Domain.Exceptions;
using BookStore.ProductService.Domain.Products;
using BookStore.ProductService.Domain.ValueObjects;

namespace BookStore.ProductService.Domain.Tests.ValueObjects;
public class ProductTests
{
    [Fact]
    public void Creating_product_with_empty_name_should_throw_domain_exception()
    {
        //Arrange
        var price=new Money(100,"USD");

        //Act
        Action action=()=>new Product ("",price);

        //Assert
        var exception=Assert.Throws<DomainException>(action);
        Assert.Equal(DomainErrors.Product.NameIsRequired,exception.Message);
    }

    [Fact]
    public void Creating_product_with_name_longer_than_200_characters_should_throw_domain_exception()
    {
        //Arrange
        var name=new string('A',201);
        var price=new Money(100,"USD");

        //Act
        Action action=()=>new Product(name,price);

        //Assert
        var exception=Assert.Throws<DomainException>(action);
        Assert.Equal(DomainErrors.Product.NameTooLong,exception.Message);
    }

    [Fact]
    public void Creating_product_with_valid_data_should_create_product_successfully()
    {
        //Arrange
        var price=new Money(100,"USD");

        var name="Clean Code Book";

        var product=new Product(name,price);

        //Act

        //Assert
        Assert.Equal(name,product.Name);
        Assert.Equal(price,product.Price);
        Assert.NotNull(product);
        Assert.IsType<Product>(product);
    }

    [Fact]
    public void Renaming_product_with_valid_name_should_change_name()
    {
        //Arrange
        var product=new Product("Old Name",new Money(100,"USD"));

        //Act
        product.Rename("New Name");

        //Asert
        Assert.Equal("New Name",product.Name);
    }
    public void renaming_product_with_empty_name_should_throw_domain_exception()
    {
        //Arrange
        var product=new Product("Old Name",new Money(100,"USD"));

        //Act
        Action action=()=>product.Rename("");

        //Assert
        var exception=Assert.Throws<DomainException>(action);

        Assert.Equal(DomainErrors.Product.NameIsRequired,exception.Message);
    }
    [Fact]
    public void Changing_product_price_should_update_price()
    {
        // Arrange
        var product = new Product(
            "Clean Code",
            new Money(100, "USD"));

        var newPrice = new Money(200, "USD");


        // Act
        product.ChangePrice(newPrice);


        // Assert
        Assert.Equal(newPrice, product.Price);
    }

    [Fact]
    public void Changing_price_of_inactive_product_should_throw_domain_exception()
    {
        //Arrange
        var product=new Product("Data Structure Book",new Money(100,"USD"));
        product.Deactivate();

        //Act
        Action action=()=>product.ChangePrice(new Money(200,"USD"));

        //Assert
        var exception=Assert.Throws<DomainException>(action);
        Assert.Equal(DomainErrors.Product.InactiveProductCannotChange,exception.Message);
    }

    [Fact]
    public void Creating_product_should_raise_product_created_event()
    {
        //Arrange
                var product=new Product("Data Structure Book",new Money(100,"USD"));

        //Act
        var events=product.DomainEvents;

        //Assert
        Assert.Contains(events,x=>x is ProductCreatedEvent);
    }
    [Fact]
    public void Renaming_product_with_valid_name_should_change_name_bdd()
    {
        //Given
        var product=new Product("Old Name",new Money(200,"EUR"));

        //When
        product.Rename("New Name");

        //Then
        Assert.Equal("New Name",product.Name);
    }
    [Fact]
    public void Renaming_product_with_empty_name_should_throw_domain_exception_bdd()
    {
        // Given
        var product = new Product(
            "Old Name",
            new Money(100, "USD"));

        // When
        Action action = () => product.Rename("");

        // Then
        var exception =
            Assert.Throws<DomainException>(action);

        Assert.Equal(
            DomainErrors.Product.NameIsRequired,
            exception.Message);
    }
    [Fact]
    public void Changing_price_should_update_product_price_bdd()
    {
        // Given
        var product = new Product(
            "Clean Code",
            new Money(100, "USD"));

        var newPrice = new Money(150, "USD");

        // When
        product.ChangePrice(newPrice);

        // Then
        Assert.Equal(newPrice, product.Price);
    }
    [Fact]
    public void Changing_price_of_inactive_product_should_throw_domain_exception_bdd()
    {
        // Given
        var product = new Product(
            "Clean Code",
            new Money(100, "USD"));

        product.Deactivate();

        // When
        Action action =
            () => product.ChangePrice(new Money(200, "USD"));

        // Then
        var exception =
            Assert.Throws<DomainException>(action);

        Assert.Equal(
            DomainErrors.Product.InactiveProductCannotChange,
            exception.Message);
    }
    [Fact]
    public void Changing_price_to_same_value_should_not_change_product_bdd()
    {
        // Given
        var price = new Money(100, "USD");

        var product = new Product(
            "Clean Code",
            price);

        // When
        product.ChangePrice(price);

        // Then
        Assert.Equal(price, product.Price);
    }

    [Fact]
    public void Creating_product_should_raise_product_created_event_bdd()
    {
        //Given
        var price = new Money(100, "USD");

        var product = new Product("Clean Code",price);

        //Then
        Assert.Single(product.DomainEvents);

        Assert.Contains(product.DomainEvents,x=>x is ProductCreatedEvent);
    }

    [Fact]
    public void Renaming_product_should_raise_product_renamed_event()
    {
        //Given
        var price = new Money(100, "USD");

        var product = new Product("Old Name",price);

        product.ClearDomainEvents();

        //When
        product.Rename("New Name");

        //Then
        Assert.Contains(product.DomainEvents,x=>x is ProductRenamedEvent);

    }

    [Fact]
public void Changing_price_should_raise_product_price_changed_event()
{
    // Given
    var product = new Product(
        "Clean Code",
        new Money(100,"USD"));

    product.ClearDomainEvents();

    // When
    product.ChangePrice(new Money(200,"USD"));

    // Then
    Assert.Single(product.DomainEvents);

    Assert.Contains(
        product.DomainEvents,
        x => x is ProductPriceChangedEvent);
}
}
