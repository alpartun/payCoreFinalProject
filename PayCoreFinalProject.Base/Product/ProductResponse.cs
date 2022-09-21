using PayCoreFinalProject.Dto;

namespace PayCoreFinalProject.Base.Product;

public class ProductResponse
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public string Brand { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public bool IsOfferable { get; set; }
    public UserDto User { get; set; }

    public ICollection<OfferDto> Offers { get; set; }
}