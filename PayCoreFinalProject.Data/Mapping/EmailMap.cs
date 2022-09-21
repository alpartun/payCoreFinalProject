using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using PayCoreFinalProject.Data.Model;

namespace PayCoreFinalProject.Data.Mapping;

public class EmailMap : ClassMapping<Email>
{
    public EmailMap()
    {
        Id(x => x.Id, x =>
        {
            x.Type(NHibernateUtil.Int32);
            x.Column("Id");
            x.UnsavedValue(0);
            x.Generator(Generators.Increment);

        });
        
        Property(x=> x.EmailAdress, x =>
        {
            x.Type(NHibernateUtil.String);
            x.NotNullable(true);
        });
        
        Property(x=> x.EmailMessage, x =>
        {
            x.Type(NHibernateUtil.String);
            x.NotNullable(true);
        });
        Property(x=> x.EmailTitle, x =>
        {
            x.Type(NHibernateUtil.String);
            x.NotNullable(true);
        });

        Property(x=> x.CreatedTime, x =>
        {
            x.Type(NHibernateUtil.DateTime);
            x.NotNullable(true);
            
        });
        Property(x=> x.SendTime, x =>
        {
            x.Type(NHibernateUtil.DateTime);
            x.NotNullable(true);
            
        });        
      
        Property(x=> x.IsSent, x =>
        {
            x.Type(NHibernateUtil.Boolean);
            x.NotNullable(true);
        });
    }
}