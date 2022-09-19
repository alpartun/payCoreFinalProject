using System.Configuration;
using PayCoreFinalProject.Base.Jwt;
using PayCoreFinalProject.StartUpExtension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddNHibernatePostgreSql(builder.Configuration.GetConnectionString("PostgreSQLConnectionString"));


MyOptions.JwtConfig = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));


builder.Services.AddServices();
builder.Services.AddJwtBearerAuthentication();
builder.Services.AddCustomizeSwagger();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseMiddleware<HeartbeatMiddleware>();


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();

class MyOptions
{
    public static JwtConfig JwtConfig { get; set; }
    
}
