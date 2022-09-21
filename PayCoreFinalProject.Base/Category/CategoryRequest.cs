using System.ComponentModel.DataAnnotations;

namespace PayCoreFinalProject.Base.Category;

public class CategoryRequest
{
    [Required] public string Name { get; set; }
}