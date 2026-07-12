using BookStore.ProductServicec.Domain.Common;
using BookStore.ProductServicec.Domain.Exceptions;
using BookStore.ProductServicec.Domain.ValueObjects;
namespace BookStore.ProductServicec.Domain.Products;
public sealed class Product:AggregatedRoot
{
    public string? Name { get; private set; }=null;
    public Money? Price { get; private set; }=null;
    public bool IsActive { get; set; }=true;

    public Product()
    {

    }
    public Product(string name,Money price)
    {
        ValidateName(name);
        Name=name.Trim();
        ChangePrice(price);
        AddDomainEvent(new ProductCreatedEvent(Id));
    }
    public void Rename(string name)
    {
        Name=name.Trim();
        Name=name;
    }
    public void ChangePrice(Money newPrice)
    {
        if(!IsActive)
            throw new DomainException(DomainErrors.Product.InactiveProductCannotChange);
        ArgumentNullException.ThrowIfNull(newPrice);
        if(Price==newPrice)
            return;
        Price=newPrice;
    }
    public static void ValidateName(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new DomainException(DomainErrors.Product.NameIsRequired);
        if(name.Length>200)
            throw new DomainException(DomainErrors.Product.NameTooLong);
    }
    public void Active()
    {
        if(IsActive)
            return;
        IsActive=true;
    }
    public void DeActive()
    {
        if(!IsActive)
            return;
        IsActive=false;
    }
}
