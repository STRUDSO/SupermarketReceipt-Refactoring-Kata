using SupermarketReceipt;

public class TenPercentDiscount : IOfferDiscountCalculator
{
    public bool Criteria(Offer offer, int quantityAsInt)
    {
        throw new System.NotImplementedException();
    }

    public Discount Discount(double quantity, double unitPrice, int numberOfXs, int quantityAsInt, Product p)
    {
        throw new System.NotImplementedException();
    }

    public Discount Discount(Offer offer, int quantityAsInt, int i, double unitPrice, double quantity, Product p)
    {
        Discount discount;
        discount = new Discount(p, offer.Argument + "% off",
            -quantity * unitPrice * offer.Argument / 100.0);
        return discount;
    }
}