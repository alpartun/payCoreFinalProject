using AutoMapper;

using PayCoreFinalProject.Service.CategoryService.Abstract;
using PayCoreFinalProject.Service.CategoryService.Concrete;
using PayCoreFinalProject.Service.Mapper;
using PayCoreFinalProject.Service.OfferService.Abstract;
using PayCoreFinalProject.Service.OfferService.Concrete;
using PayCoreFinalProject.Service.ProductService.Abstract;
using PayCoreFinalProject.Service.ProductService.Concrete;
using PayCoreFinalProject.Service.RegisterService.Abstract;
using PayCoreFinalProject.Service.RegisterService.Concrete;
using PayCoreFinalProject.Service.Token.Abstract;
using PayCoreFinalProject.Service.Token.Concrete;
using PayCoreFinalProject.Service.UserService.Abstract;
using PayCoreFinalProject.Service.UserService.Concrete;

namespace PayCoreFinalProject.StartUpExtension;

public static class ExtensionService
{
    /*public static void AddRedisDependencyInjection(this IServiceCollection services, IConfiguration Configuration)
    {
        //redis 
        var configurationOptions = new ConfigurationOptions();
        configurationOptions.EndPoints.Add(Configuration["Redis:Host"], Convert.ToInt32(Configuration["Redis:Port"]));
        int.TryParse(Configuration["Redis:DefaultDatabase"], out int defaultDatabase);
        configurationOptions.DefaultDatabase = defaultDatabase;
        services.AddStackExchangeRedisCache(options =>
        {
            options.ConfigurationOptions = configurationOptions;
            options.InstanceName = Configuration["Redis:InstanceName"];
        });
    }*/

    public static void AddServices(this IServiceCollection services)
    {
        // services 
        services.AddScoped<IRegisterService,RegisterService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOfferService, OfferService>();
        services.AddHttpContextAccessor();



        // mapper
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        services.AddSingleton(mapperConfig.CreateMapper());
    }
}