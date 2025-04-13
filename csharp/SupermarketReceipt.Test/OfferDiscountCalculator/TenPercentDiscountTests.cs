using System;
using Xunit;

namespace SupermarketReceipt.Test.OfferDiscountCalculator;

public class TenPercentDiscountTests
{
    [Fact]
    public void Discount_Calculation()
    {
        var sut = new TenPercentDiscount();
        var p = new Product(Guid.NewGuid().ToString(), ProductUnit.Kilo);
        var offer = new Offer(SpecialOfferType.TenPercentDiscount, p, 10);
        var unitPrice = 10;
        var quantity = 2;
        var discount = sut.Discount(offer, -1, -1, unitPrice, quantity, p);

        var expected = -quantity * unitPrice * offer.Argument / 100.0;
        Assert.Equal(expected, discount.DiscountAmount);
        Assert.Equal(offer.Argument + "% off", discount.Description);

        Assert.Throws<NotImplementedException>(() => sut.Discount(quantity, unitPrice, 3, 3, p));
        Assert.Throws<NotImplementedException>(() => sut.Criteria(offer, unitPrice));
    }
}