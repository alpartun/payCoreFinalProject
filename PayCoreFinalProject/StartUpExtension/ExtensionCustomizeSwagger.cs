using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace PayCoreFinalProject.StartUpExtension;

public static class ExtensionCustomizeSwagger
{
    //Customize swagger for jwt token
    public static void AddCustomizeSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "PayCore Final Project API", Version = "v1.0" });
            c.OperationFilter<ExtensionSwaggerFileOperationFilter>();

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "PayCore Final Project",
                Description = "Enter JWT Bearer token *only*",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, new string[] { } }
            });
        });
    }
}