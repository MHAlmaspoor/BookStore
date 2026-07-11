using BookStore.ProductServicec.Domain.Common;
using BookStore.ProductServicec.Domain.Exceptions;
using BookStore.ProductServicec.Domain.ValueObjects;
namespace BookStore.ProductServicec.Domain.Products;
public sealed class Product:AggregatedRoot
{
    public string? Name { get; private set; }=null;
    public Money? Price { get; private set; }=null;

    public Product()
    {

    }
    public Product(string name,Money price)
    {
        Rename(name);
        ChangePrice(price);
        AddDomainEvent(new ProductCreatedEvent(Id));
    }
    public void Rename(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name is required. ");

        if(name.Length>200)
            throw new DomainException("Product name is too large. ");
        Name=name;
    }
    public void ChangePrice(Money newPrice)
    {
        ArgumentNullException.ThrowIfNull(newPrice);
        Price=newPrice;
    }
}
