using System.Collections.Generic;
using System.Globalization;

namespace SupermarketReceipt;

public interface IDiscountCalculator
{
    IEnumerable<Discount> CalculateDiscount(Dictionary<Product, Offer> offers,
        Dictionary<Product, double> productQuantities);
}

public class DiscountCalculator : IDiscountCalculator
{
    private readonly ISupermarketCatalog _catalog;
    private readonly IOfferDiscountCalculator _threeForTwoDiscount;
    private readonly IOfferDiscountCalculator _twoForAmountDiscount;
    private readonly IOfferDiscountCalculator _tenPercentDiscount;
    private readonly IOfferDiscountCalculator _fiveForAmountDiscount;

    public DiscountCalculator(ISupermarketCatalog catalog) : this(catalog, new ThreeForTwoDiscount(),
        new TwoForAmountDiscount(), new TenPercentDiscount(), new FiveForAmountDiscount()){}
    public DiscountCalculator(ISupermarketCatalog catalog, IOfferDiscountCalculator threeForTwoDiscount, IOfferDiscountCalculator twoForAmountDiscount, IOfferDiscountCalculator tenPercentDiscount, IOfferDiscountCalculator fiveForAmountDiscount)
    {
        _catalog = catalog;
        _threeForTwoDiscount = threeForTwoDiscount;
        _twoForAmountDiscount = twoForAmountDiscount;
        _tenPercentDiscount = tenPercentDiscount;
        _fiveForAmountDiscount = fiveForAmountDiscount;
    }
    private static readonly CultureInfo Culture = CultureInfo.CreateSpecificCulture("en-GB");

    public IEnumerable<Discount> CalculateDiscount(Dictionary<Product, Offer> offers,
        Dictionary<Product, double> productQuantities)
    {
        foreach (var p in productQuantities.Keys)
        {
            var quantity = productQuantities[p];
            var quantityAsInt = (int)quantity;
            if (offers.ContainsKey(p))
            {
                var offer = offers[p];
                var unitPrice = _catalog.GetUnitPrice(p);
                Discount discount = null;
                var x = 1;
                if (_threeForTwoDiscount.Criteria(offer, quantityAsInt))
                {
                    x = 3;
                }
                else if (_twoForAmountDiscount.Criteria(offer, quantityAsInt))
                {
                    x = 2;
                    if (quantityAsInt >= 2)
                    {
                        discount = _twoForAmountDiscount.Discount(offer, quantityAsInt, x, unitPrice, quantity, p);
                    }
                }

                if (offer.OfferType == SpecialOfferType.FiveForAmount) x = 5;
                var numberOfXs = quantityAsInt / x;
                if (ThreeForTwoCriteria(offer, quantityAsInt))
                {
                    discount =_threeForTwoDiscount.Discount(quantity, unitPrice, numberOfXs, quantityAsInt, p);
                }

                if (offer.OfferType == SpecialOfferType.TenPercentDiscount)
                    discount = _tenPercentDiscount.Discount(offer, quantityAsInt, x, unitPrice, quantity, p);
                if (_fiveForAmountDiscount.Criteria(offer, quantityAsInt))
                {
                    discount = FiveForAmountDiscount(unitPrice, quantity, offer, numberOfXs, quantityAsInt, p, x);
                }

                if (discount != null)
                    yield return discount;
            }
        }
    }

    private bool ThreeForTwoCriteria(Offer offer, int quantityAsInt)
    {
        return offer.OfferType == SpecialOfferType.ThreeForTwo && quantityAsInt > 2;
    }

    private Discount FiveForAmountDiscount(double unitPrice, double quantity, Offer offer, int numberOfXs,
        int quantityAsInt, Product p, int x)
    {
        Discount discount;
        var discountTotal = unitPrice * quantity -
                            (offer.Argument * numberOfXs + quantityAsInt % 5 * unitPrice);
        discount = new Discount(p, x + " for " + PrintPrice(offer.Argument), -discountTotal);
        return discount;
    }

    public static string PrintPrice(double price)
    {
        return price.ToString("N2", Culture);
    }
}



