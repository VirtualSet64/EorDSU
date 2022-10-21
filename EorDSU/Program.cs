using EorDSU;
using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sentry;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<BASEPERSONMDFContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BasePerson")));
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.WebHost.ConfigureServices(configure => SentrySdk.Init(o =>
{
    // Tells which project in Sentry to send events to:
    o.Dsn = "https://f2dd48eaeae94c1cac84df1ec97d899b@o4503999231623168.ingest.sentry.io/4503999239094272";
    // When configuring for the first time, to see what the SDK is doing:
    o.Debug = true;
    // Set traces_sample_rate to 1.0 to capture 100% of transactions for performance monitoring.
    // We recommend adjusting this value in production.
    o.TracesSampleRate = 1.0;
    // Enable Global Mode if running in a client app
    o.IsGlobalModeEnabled = true;
}));


builder.Services.AddIdentity<EorDSU.Models.User, IdentityRole>(opts =>
{
    opts.Password.RequiredLength = 0;   // минимальная длина
    opts.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
    opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
    opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
    opts.Password.RequireDigit = false; // требуются ли цифры
}).AddEntityFrameworkStores<ApplicationContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
