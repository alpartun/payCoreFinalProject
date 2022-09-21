using PayCoreFinalProject.Base.Jwt;
using PayCoreFinalProject.Data.Model;
using PayCoreFinalProject.StartUpExtension;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

Options.env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
// Add services to the container.

var configuration = new ConfigurationBuilder()
    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Options.env}.json", optional: true)
    .Build();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information(("Application starting..."));
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//handle circle loop
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddNHibernatePostgreSql(builder.Configuration.GetConnectionString("PostgreSQLConnectionString"));


Options.JwtConfig = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
builder.Configuration.GetSection("Email").Get<EmailSettings>();
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection(EmailSettings.Email));
builder.Configuration.GetSection("RabbitMq").Get<RabbitMqSettings>();
builder.Services.Configure<RabbitMqSettings>(
    builder.Configuration.GetSection(RabbitMqSettings.RabbitMq));

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



app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();

class Options
{
    public static JwtConfig JwtConfig { get; set; }
    public static string env { get; set; }
}