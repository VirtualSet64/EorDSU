using BasePersonDBService.DataContext;
using DSUContextDBService.DataContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sentry;
using DomainServices.DBService;
using SvedenOop.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowCredentialsPolicy",
        policy =>
        {
            policy.WithOrigins(builder.Configuration["LinkToFrontEnd"])
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

builder.Services.AddDbContext<BASEPERSONMDFContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BasePerson"), providerOptions => providerOptions.EnableRetryOnFailure()), ServiceLifetime.Scoped);
builder.Services.AddDbContext<DSUContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BaseDekanat"), providerOptions => providerOptions.EnableRetryOnFailure()), ServiceLifetime.Scoped);
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SvedenOop"), providerOptions => providerOptions.EnableRetryOnFailure()), ServiceLifetime.Scoped);

builder.Services.AddIdentity<DomainServices.Entities.User, IdentityRole>(
               opts =>
               {
                   opts.Password.RequiredLength = 2;
                   opts.Password.RequireNonAlphanumeric = false;
                   opts.Password.RequireLowercase = false;
                   opts.Password.RequireUppercase = false;
                   opts.Password.RequireDigit = false;
               })
               .AddEntityFrameworkStores<ApplicationContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;

    options.LoginPath = "/Account/Login";
    options.SlidingExpiration = true;
});

builder.WebHost.ConfigureServices(configure => SentrySdk.Init(o =>
{
    // Tells which project in Sentry to send events to:
    o.Dsn = builder.Configuration["SentyDSN"]; ;
    // When configuring for the first time, to see what the SDK is doing:
    o.Debug = true;
    // Set traces_sample_rate to 1.0 to capture 100% of transactions for performance monitoring.
    // We recommend adjusting this value in production.
    o.TracesSampleRate = 1.0;
    // Enable Global Mode if running in a client app
    o.IsGlobalModeEnabled = true;
}));

builder.Services.AddDBService();

builder.Services.AddAuthorization();

var app = builder.Build();

app.ConfigureExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyAllowCredentialsPolicy");

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();