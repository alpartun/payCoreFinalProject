using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using PayCoreFinalProject.Data.Model;

namespace PayCoreFinalProject.Data.Mapping;

public class UserMap : ClassMapping<User>
{
    public UserMap()
    {
        Id(x => x.Id, x =>
        {
            x.Type(NHibernateUtil.Int32);
            x.Column("Id");
            x.UnsavedValue(0);
            x.Generator(Generators.Increment);

        });
        
        Property(x=> x.Name, x =>
        {
            x.Length(50);
            x.Type(NHibernateUtil.String);
            x.NotNullable(true);
        });
        
        Property(x=> x.Surname, x =>
        {
            x.Length(50);
            x.Type(NHibernateUtil.String);
            x.NotNullable(true);
        });
        Property(x=> x.Email, x =>
        {
            x.Length(50);
            x.Type(NHibernateUtil.String);
            x.NotNullable(true);
        });
        Property(x=> x.PasswordHash, x =>
        {
            x.Length(50);
            x.Type(NHibernateUtil.BinaryBlob);
            x.NotNullable(true);
        });
        Property(x=> x.PasswordSalt, x =>
        {
            x.Length(50);
            x.Type(NHibernateUtil.BinaryBlob);
            x.NotNullable(true);
            
        });
        Bag<Product>(x => x.Products, c => { }, cr => cr.OneToMany(x => x.Class(typeof(Product))));
        Bag<Offer>(x => x.Offers, c => { }, cr => cr.OneToMany(x => x.Class(typeof(Offer))));

        
        Table("user");
        
    }
}