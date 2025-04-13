using SupermarketReceipt;

public class FiveForAmountDiscount : IOfferDiscountCalculator
{
    public bool Criteria(Offer offer, int quantityAsInt)
    {
        return offer.OfferType == SpecialOfferType.FiveForAmount && quantityAsInt >= 5;
    }

    public Discount Discount(double quantity, double unitPrice, int numberOfXs, int quantityAsInt, Product p)
    {
        throw new System.NotImplementedException();
    }

    public Discount Discount(Offer offer, int quantityAsInt, int i, double unitPrice, double quantity, Product p)
    {
        throw new System.NotImplementedException();
    }
}

public interface IOfferDiscountCalculator
{
    bool Criteria(Offer offer, int quantityAsInt);
    Discount Discount(double quantity, double unitPrice, int numberOfXs, int quantityAsInt, Product p);
    Discount Discount(Offer offer, int quantityAsInt, int i, double unitPrice, double quantity, Product p);
}

public class ThreeForTwoDiscount : IOfferDiscountCalculator
{
    public bool Criteria(Offer offer, int quantityAsInt)
    {
        return offer.OfferType == SpecialOfferType.ThreeForTwo;
    }

    public Discount Discount(double quantity, double unitPrice, int numberOfXs, int quantityAsInt,
        Product p)
    {
        Discount discount;
        var discountAmount = quantity * unitPrice -
                             (numberOfXs * 2 * unitPrice + quantityAsInt % 3 * unitPrice);
        discount = new Discount(p, "3 for 2", -discountAmount);
        return discount;
    }

    public Discount Discount(Offer offer, int quantityAsInt, int i, double unitPrice, double quantity, Product p)
    {
        throw new System.NotImplementedException();
    }
}