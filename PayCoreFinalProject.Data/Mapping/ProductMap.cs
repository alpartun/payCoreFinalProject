using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using PayCoreFinalProject.Data.Model;

namespace PayCoreFinalProject.Data.Mapping;

public class ProductMap : ClassMapping<Product>
{
    public ProductMap()
    {
        Id(x => x.Id, x =>
        {
            x.Type(NHibernateUtil.Int32);
            x.Column("Id");
            x.UnsavedValue(0);
            x.Generator(Generators.Increment);
        });

        Property(x => x.Name, x =>
        {
            x.Length(100);
            x.Type(NHibernateUtil.String);
            x.NotNullable(true);
        });

        Property(x => x.Color, x =>
        {
            x.Type(NHibernateUtil.String);
            x.NotNullable(true);
        });
        Property(x => x.Brand, x =>
        {
            x.Type(NHibernateUtil.String);
            x.NotNullable(true);
        });
        Property(x => x.Price, x =>
        {
            x.Type(NHibernateUtil.Decimal);
            x.Precision(10);
            x.Scale(2);
        });
        Property(x => x.Description, x =>
        {
            x.Length(500);
            x.Type(NHibernateUtil.String);
            x.NotNullable(true);
        });
        Property(x => x.IsOfferable, x =>
        {
            x.Type(NHibernateUtil.Boolean);
            x.NotNullable(true);
        });
        Property(x => x.IsSold, x =>
        {
            x.Type(NHibernateUtil.Boolean);
            x.NotNullable(true);
        });

        ManyToOne(x => x.User);
        ManyToOne(x => x.Category);
        Bag<Offer>(x => x.Offers, c => { }, cr => cr.OneToMany(x => x.Class(typeof(Offer))));


        Table("product");
    }
}