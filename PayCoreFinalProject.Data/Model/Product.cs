namespace PayCoreFinalProject.Data.Model;

public class Product
{
    public virtual int Id { get; set; }
    public virtual string Name { get; set; }
    public virtual string Color { get; set; }
    public virtual string Brand { get; set; }
    public virtual decimal Price { get; set; }
    public virtual string Description { get; set; }
    public virtual bool IsOfferable { get; set; }
    public virtual bool IsSold { get; set; } = false;
    public virtual Category Category { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<Offer> Offers { get; set; }
}