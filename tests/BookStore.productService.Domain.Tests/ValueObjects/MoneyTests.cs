using BookStore.ProductServicec.Domain.Common;
using BookStore.ProductServicec.Domain.Exceptions;
using BookStore.ProductServicec.Domain.ValueObjects;

namespace BookStore.ProductService.Domain.Tests.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Two_money_objects_with_same_amount_and_currency_should_be_equal()
    {
        //Arrange
        var money1=new Money(100,"USD");
        var money2=new Money(100,"USD");

        //Act

        //Assert
        Assert.Equal(money1,money2);
    }

    [Fact]
    public void Two_money_objects_with_diffrent_amount_should_not_be_equals()
    {
        //Arrange
        var money1=new Money(100,"USD");
        var money2=new Money(200,"USD");

        //Act

        //Assert
        Assert.NotEqual(money1,money2);
    }

    [Fact]
    public void Two_money_objects_with_diffrent_currency_should_not_be_equals()
    {
        //Arrange
        var money1=new Money(100,"USD");
        var money2=new Money(100,"EUR");

        //Act

        //Assert
        Assert.NotEqual(money1,money2);
    }

    [Fact]
    public void Creating_money_with_neqative_amount_should_be_throw_domin_exception()
    {
        //Arrange
        Action action=()=>new Money(-100,"USD");
        var exception=Assert.Throws<DomainException>(action);

        //Act

        //Assert

        Assert.Equal(DomainErrors.Money.NegativeAmount,exception.Message);
    }
}
