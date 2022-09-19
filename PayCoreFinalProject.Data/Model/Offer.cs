namespace PayCoreFinalProject.Data.Model;

public class Offer
{
    public virtual int Id { get; set; }
    public virtual User User { get; set; }
    public virtual Product Product { get; set; }
    public virtual double OfferedPrice { get; set; }
    public virtual int OfferedById { get; set; }
}