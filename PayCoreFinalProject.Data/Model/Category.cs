namespace PayCoreFinalProject.Data.Model;

public class Category
{
    public virtual int Id { get; set; }
    public virtual string Name { get; set; }
    public virtual ICollection<Product> Products { get; set; }
}