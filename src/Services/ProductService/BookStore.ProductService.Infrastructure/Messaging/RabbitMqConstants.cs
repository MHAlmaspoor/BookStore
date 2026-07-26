namespace BookStore.ProductService.Infrastructure.Messaging;

public static class RabbitMqConstants
{
    public const string ExchangeName="bookshop.events";

    public static class RoutingKeys
    {
        public const string ProductCreated = "product.created";
        public const string ProductUpdated = "product.updated";
        public const string ProductDeleted = "product.deleted";


    }
}
