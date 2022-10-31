using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;
using EorDSU.Repository;
using EorDSU.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sentry;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EorDSU.ConfigInfo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BASEPERSONMDFContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BasePerson"), providerOptions => providerOptions.EnableRetryOnFailure()));
builder.Services.AddDbContext<DSUContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BaseDekanat"), providerOptions => providerOptions.EnableRetryOnFailure()));
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), providerOptions => providerOptions.EnableRetryOnFailure()));

builder.Services.AddScoped<IActiveData, ActiveDataRepository>();
builder.Services.AddScoped<ISearchEntity, SearchEntityRepository>();
builder.Services.AddScoped<ParsingService>();
builder.Services.AddSingleton<AuthOptions>();

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

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // указывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = builder.Configuration.GetConnectionString("ISSUER"),
            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = builder.Configuration.GetConnectionString("AUDIENCE"),
            // будет ли валидироваться время существования
            ValidateLifetime = true,
            // установка ключа безопасности
            IssuerSigningKey = new AuthOptions(builder.Configuration).GetSymmetricSecurityKey(),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();