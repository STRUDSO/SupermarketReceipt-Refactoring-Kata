using System;
using Xunit;

namespace SupermarketReceipt.Test.OfferDiscountCalculator;

public class DicountCalculationTests
{
    [Fact]
    public void TenPercentDiscount()
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

    [Fact]
    public void TwoForAmountDiscount()
    {
        var sut = new TwoForAmountDiscount();
        var p = new Product(Guid.NewGuid().ToString(), ProductUnit.Kilo);
        var offer = new Offer(SpecialOfferType.TwoForAmount, p, 10);
        var unitPrice = 10;
        var quantityAsInt = 2;
        var x = 3;
        var discount = sut.Discount(offer, quantityAsInt, x, unitPrice, quantityAsInt, p);

        var total = offer.Argument * (quantityAsInt / x) + quantityAsInt % 2 * unitPrice;
        var expected = unitPrice * quantityAsInt - total;
        Assert.Equal(-expected, discount.DiscountAmount);
        Assert.Equal("2 for " + DiscountCalculator.PrintPrice(offer.Argument), discount.Description);

        Assert.Throws<NotImplementedException>(() => sut.Discount(quantityAsInt, unitPrice, 3, 3, p));
        Assert.True(sut.Criteria(offer, unitPrice));
    }
}