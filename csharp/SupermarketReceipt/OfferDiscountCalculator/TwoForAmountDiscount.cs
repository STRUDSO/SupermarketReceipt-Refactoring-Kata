using SupermarketReceipt;

public class TwoForAmountDiscount : IOfferDiscountCalculator
{
    public bool Criteria(Offer offer, int quantityAsInt)
    {
        return offer.OfferType == SpecialOfferType.TwoForAmount;
    }

    public Discount Discount(double quantity, double unitPrice, int numberOfXs, int quantityAsInt, Product p)
    {
        throw new System.NotImplementedException();
    }

    public Discount Discount(Offer offer, int quantityAsInt, int x, double unitPrice, double quantity,
        Product p)
    {
        Discount discount;
        var total = offer.Argument * (quantityAsInt / x) + quantityAsInt % 2 * unitPrice;
        var discountN = unitPrice * quantity - total;
        discount = new Discount(p, "2 for " + DiscountCalculator.PrintPrice(offer.Argument), -discountN);
        return discount;
    }
}