namespace PayCoreFinalProject.Data.Model;

public class User
{
    public virtual int Id { get; set; }
    public virtual string Name { get; set; }
    public virtual string Surname { get; set; }
    public virtual byte[] PasswordHash { get; set; }
    public virtual byte[] PasswordSalt { get; set; }
    public virtual string Email { get; set; }

    public virtual ICollection<Product> Products { get; set; }
    public virtual ICollection<Offer> Offers { get; set; }
}