using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Xunit;
using Assert = Xunit.Assert;

namespace SupermarketReceipt.Test;

public class DiscountCalculatorTests
{
    [Fact]
    public void Discount_CalculationCalls_OfferDiscountCalculator()
    {
        var random = new Random();
        var randomString = () => Guid.NewGuid().ToString();
        var quantity = Math.Abs(random.NextDouble() * 10);
        var offerMock = new Mock<IOfferDiscountCalculator>();
        offerMock.Setup(x => x.Criteria(It.IsAny<Offer>(), It.IsAny<int>())).Returns(true);
        offerMock.SetReturnsDefault(new Offer(SpecialOfferType.FiveForAmount, new Product(randomString(), ProductUnit.Each), -1));
        var discountCalculator = new DiscountCalculator(Mock.Of<ISupermarketCatalog>(), offerMock.Object, offerMock.Object,offerMock.Object,offerMock.Object);
        var threeForTwo = new {quantity= quantity + 1, p = new Product(randomString(), ProductUnit.Each)};
        var fiveForAmount = new {quantity= quantity + 2, p = new Product(randomString(), ProductUnit.Kilo)};
        var noOffers = new {quantity= quantity + 3, p = new Product(randomString(), ProductUnit.Each)};

        var offers = new Dictionary<Product, Offer>
        {
            { threeForTwo.p, new Offer(SpecialOfferType.ThreeForTwo, threeForTwo.p, 0) },
            { fiveForAmount.p, new Offer(SpecialOfferType.FiveForAmount, fiveForAmount.p, 0) }
        };
        var productQuantities = new Dictionary<Product, double>
        {
            { threeForTwo.p, threeForTwo.quantity + 3 },
            { fiveForAmount.p, fiveForAmount.quantity + 5 },
            { noOffers.p, noOffers.quantity },
        };

        // ACT
        var discounts = discountCalculator.CalculateDiscount(offers, productQuantities).ToList();


        // ASSERT
        offerMock.Verify(x => x.Criteria(It.IsAny<Offer>(), (int)threeForTwo.quantity + 3), Times.AtLeastOnce);
        offerMock.Verify(x => x.Criteria(It.IsAny<Offer>(), (int)fiveForAmount.quantity + 5), Times.AtLeastOnce);
        offerMock.Verify(x => x.Criteria(It.IsAny<Offer>(), (int)noOffers.quantity), Times.Never);
        Assert.Equal(2, discounts.Count);
    }

    [Fact]
    public void TwoForAmountDiscountInvocation()
    {
        var random = new Random();
        var randomString = () => Guid.NewGuid().ToString();
        var quantity = Math.Abs(random.NextDouble() * 10);
        var offerMock = new Mock<IOfferDiscountCalculator>();
        offerMock.Setup(x => x.Criteria(It.IsAny<Offer>(), It.IsAny<int>())).Returns(true);
        offerMock.SetReturnsDefault(new Offer(SpecialOfferType.FiveForAmount, new Product(randomString(), ProductUnit.Each), -1));

        var discountCalculator = new DiscountCalculator(Mock.Of<ISupermarketCatalog>(), Mock.Of<IOfferDiscountCalculator>(), offerMock.Object,offerMock.Object,offerMock.Object);
        var threeForTwoDiscount = new {quantity= quantity + 1, p = new Product(randomString(), ProductUnit.Each)};
        var fiveForAmount = new {quantity= quantity + 2, p = new Product(randomString(), ProductUnit.Kilo)};
        var noOffers = new {quantity= quantity + 3, p = new Product(randomString(), ProductUnit.Each)};

        var offers = new Dictionary<Product, Offer>
        {
            { threeForTwoDiscount.p, new Offer(SpecialOfferType.TwoForAmount, threeForTwoDiscount.p, 0) },
            { fiveForAmount.p, new Offer(SpecialOfferType.TenPercentDiscount, fiveForAmount.p, 0) }
        };
        var productQuantities = new Dictionary<Product, double>
        {
            { threeForTwoDiscount.p, threeForTwoDiscount.quantity + 3 },
            { fiveForAmount.p, fiveForAmount.quantity + 5 },
            { noOffers.p, noOffers.quantity },
        };

        // ACT
        var discounts = discountCalculator.CalculateDiscount(offers, productQuantities).ToList();


        // ASSERT
        offerMock.Verify(x => x.Criteria(It.IsAny<Offer>(), (int)threeForTwoDiscount.quantity + 3), Times.AtLeastOnce);
        offerMock.Verify(x => x.Criteria(It.IsAny<Offer>(), (int)fiveForAmount.quantity + 5), Times.AtLeastOnce);
        offerMock.Verify(x => x.Criteria(It.IsAny<Offer>(), (int)noOffers.quantity), Times.Never);
        Assert.Equal(2, discounts.Count);
    }

    [Fact]
    public void Ensure_CoverageFor_Ctor()
    {
        Assert.NotNull(new DiscountCalculator(Mock.Of<ISupermarketCatalog>()));
    }
}