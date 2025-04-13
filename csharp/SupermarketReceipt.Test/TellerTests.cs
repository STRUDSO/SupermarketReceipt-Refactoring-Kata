using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Xunit;
using Assert = Xunit.Assert;

namespace SupermarketReceipt.Test;

public class TellerTests
{
    [Fact]
    public void Teller_Will_AddDiscount()
    {
        var quantity = new Random().NextDouble()*10;
        var amount = new Random().NextDouble()*10;
        var product = new Product(Guid.NewGuid().ToString(), ProductUnit.Each);
        var discount = new Discount(product, "", amount);
        ICollection<Discount> discounts = [discount];
        var teller = new Teller(Mock.Of<ISupermarketCatalog>(), Mock.Of<IDiscountCalculator>(x =>
            x.CalculateDiscount(It.IsAny<Dictionary<Product, Offer>>(), It.IsAny<Dictionary<Product, double>>()) == discounts));

        var shoppingCart = new ShoppingCart();
        shoppingCart.AddItemQuantity(product, quantity);

        var receipt = teller.ChecksOutArticlesFrom(shoppingCart);

        var actualDiscount = Assert.Single(receipt.GetDiscounts());
        Assert.Equal(product, actualDiscount.Product);
        Assert.Equal(amount, actualDiscount.DiscountAmount);

    }
}