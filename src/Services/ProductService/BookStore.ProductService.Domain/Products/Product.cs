using BookStore.ProductService.Domain.Common;
using BookStore.ProductService.Domain.Exceptions;
using BookStore.ProductService.Domain.ValueObjects;
using BookStore.BuildingBlocks.Domain;

namespace BookStore.ProductService.Domain.Products;
public sealed class Product:AggregatedRoot
{
    public string Name { get; private set; }
    public Money Price { get; private set; }
    public bool IsActive { get; private set; }=true;

    private Product()
    {

    }
    public Product(string name,Money price)
    {
        SetInitialName(name);
        SetInitialPrice(price);
        AddDomainEvent(new ProductCreatedEvent(Id,Name, price.Amount, price.Currency));
    }
    public void Rename(string name)
    {
        ValidateName(name);
        Name=name.Trim();
        AddDomainEvent(new ProductRenamedEvent(Id));
    }
    public void ChangePrice(Money newPrice)
    {
        ArgumentNullException.ThrowIfNull(newPrice);

        if(!IsActive)
            throw new DomainException(DomainErrors.Product.InactiveProductCannotChange);

        if(Price==newPrice)
            return;
        Price=newPrice;
        AddDomainEvent(new ProductPriceChangedEvent(Id));
    }
    private static void ValidateName(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new DomainException(DomainErrors.Product.NameIsRequired);
        if(name.Length>200)
            throw new DomainException(DomainErrors.Product.NameTooLong);
    }
    public void Activate()
    {
        if(IsActive)
            return;
        IsActive=true;
    }
    public void Deactivate()
    {
        if(!IsActive)
            return;
        IsActive=false;
    }
    private void SetInitialPrice(Money price)
    {
        ArgumentNullException.ThrowIfNull(price);

        Price = price;
    }
    private void SetInitialName(string name)
    {
        ValidateName(name);
        Name=name.Trim();
    }
}
