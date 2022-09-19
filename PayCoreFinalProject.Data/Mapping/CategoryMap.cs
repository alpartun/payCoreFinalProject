using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using PayCoreFinalProject.Data.Model;

namespace PayCoreFinalProject.Data.Mapping;

public class CategoryMap : ClassMapping<Category>
{
    public CategoryMap()
    {
        Table("category");
        
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
        
        Bag<Product>(x => x.Products, c => { }, cr => cr.OneToMany(x => x.Class(typeof(Product))));

    }
    
}