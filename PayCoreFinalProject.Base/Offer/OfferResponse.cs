using PayCoreFinalProject.Dto;

namespace PayCoreFinalProject.Base.Offer;

public class OfferResponse
{
    public int ProductId { get; set; }
    public  double OfferedPrice { get; set; }
    public  ProductDto Product { get; set; }


}