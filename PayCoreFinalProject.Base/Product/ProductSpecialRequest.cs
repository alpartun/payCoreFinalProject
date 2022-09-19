using System.Text.Json.Serialization;

namespace PayCoreFinalProject.Base.Product;

public class ProductSpecialRequest
{
    [JsonIgnore]
    public int Id { get; set; }
    public  int CategoryId { get; set; }
    public  string Name { get; set; }
    public  string Color { get; set; }
    public  string Brand { get; set; }
    public  double Price { get; set; }
    public  bool IsOfferable { get; set; }
    public  string Description { get; set; }
}