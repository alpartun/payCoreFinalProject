using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernate.Dialect;
using PayCoreFinalProject.Data.Mapping;

namespace PayCoreFinalProject.StartUpExtension;

public static class ExtensionNHibernate
{
    // NHibernate 
    public static IServiceCollection AddNHibernatePostgreSql(this IServiceCollection services, string connectionString)
    {
        var mapper = new ModelMapper();
        mapper.AddMappings(typeof(DefaultMapping).Assembly.ExportedTypes);
        HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

        var configuration = new Configuration();
        configuration.DataBaseIntegration(c =>
        {
            c.Dialect<PostgreSQLDialect>();
            c.ConnectionString = connectionString;
            c.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
            c.SchemaAction = SchemaAutoAction.Update;
            c.LogFormattedSql = true;
            c.LogSqlInConsole = true;
        });
        configuration.AddMapping(domainMapping);

        var sessionFactory = configuration.BuildSessionFactory();

        services.AddSingleton(sessionFactory);
        services.AddScoped(factory => sessionFactory.OpenSession());

        return services;
    }
}