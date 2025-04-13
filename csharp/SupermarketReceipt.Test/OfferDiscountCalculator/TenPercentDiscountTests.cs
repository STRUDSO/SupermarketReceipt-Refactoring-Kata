using System;
using Xunit;

namespace SupermarketReceipt.Test.OfferDiscountCalculator;

public class TenPercentDiscountTests
{
    [Fact]
    public void Discount_Calculation()
    {
        var tenPercentDiscount = new TenPercentDiscount();
        var product = new Product(Guid.NewGuid().ToString(), ProductUnit.Kilo);
        var offer = new Offer(SpecialOfferType.TenPercentDiscount, product, 10);
        var unitPrice = 10;
        var quantity = 2;
        var discount = tenPercentDiscount.Discount(offer, -1, -1, unitPrice, quantity, product);

        var expected = -quantity * unitPrice * offer.Argument / 100.0;
        Assert.Equal(expected, discount.DiscountAmount);
        Assert.Equal(offer.Argument + "% off", discount.Description);
    }
}