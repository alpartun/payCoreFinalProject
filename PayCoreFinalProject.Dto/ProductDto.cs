using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PayCoreFinalProject.Dto;

public class ProductDto
{
    public  CategoryDto Category { get; set; }
    public int Id { get; set; }

    public  string Name { get; set; }
    public  string Color { get; set; }
    public  string Brand { get; set; }
    public  double Price { get; set; }
    public  bool IsOfferable { get; set; }

    public  string Description { get; set; }



}