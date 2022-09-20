using System.ComponentModel.DataAnnotations;

namespace PayCoreFinalProject.Base.Product;

public class ProductRequest
{
    [Required]
    public  int CategoryId { get; set; }
    [Required]

    public  string Name { get; set; }
    [Required]

    public  string Color { get; set; }
    [Required]

    public  string Brand { get; set; }
    [Required]

    public  double Price { get; set; }
    [Required]

    public  bool IsOfferable { get; set; }
    [Required]

    public  string Description { get; set; }
}