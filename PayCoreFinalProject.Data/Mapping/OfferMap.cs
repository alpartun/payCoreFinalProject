using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using PayCoreFinalProject.Data.Model;

namespace PayCoreFinalProject.Data.Mapping;

public class OfferMap : ClassMapping<Offer>
{
    public OfferMap()
    {
        Id(x => x.Id, x =>
        {
            x.Type(NHibernateUtil.Int32);
            x.Column("Id");
            x.UnsavedValue(0);
            x.Generator(Generators.Increment);

        });
        
        Property(x=> x.OfferedPrice, x =>
        {
            x.Type(NHibernateUtil.Decimal);
            x.Precision(10);
            x.Scale(2);
            
        });
        Property(x=> x.OfferedById, x =>
        {
            x.Type(NHibernateUtil.Int32);
            x.NotNullable(true);
        });
        ManyToOne(x=>x.User);
        ManyToOne(x=>x.Product);

    }
}