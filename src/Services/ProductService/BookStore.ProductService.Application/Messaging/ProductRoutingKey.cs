namespace BookStore.ProductService.Application.Messaging;

public static class ProductRoutingKeys
{
    public const string Created = "product.created";
    public const string Updated = "product.updated";
    public const string Deleted = "product.deleted";
}
