using System.ComponentModel.DataAnnotations;

namespace PayCoreFinalProject.Base.Product;

public class ProductRequest
{
    [Required] public int CategoryId { get; set; }

    [Required]
    [StringLength(maximumLength: 20)]
    public string Name { get; set; }

    [Required] public string Color { get; set; }
    [Required] public string Brand { get; set; }
    [Required] public decimal Price { get; set; }
    [Required] public bool IsOfferable { get; set; }

    [Required]
    [StringLength(maximumLength: 500)]

    public string Description { get; set; }
}