
namespace BookStore.ProductServicec.Domain.Common;

public static class DomainErrors
{
    public static class Product
    {
        public const string NameIsRequired =
            "Product name is required.";

        public const string NameTooLong =
            "Product name cannot exceed 200 characters.";

        public const string InactiveProductCannotChange=
            "Inactive products cannot chance price. ";

    }
    public static class Money
    {
        public const string NegativeAmount =
            "Amount cannot be negetive. ";

        public const string InvalidCurrency=
            "Currency is not currect. ";
    }
}
