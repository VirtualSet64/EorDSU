using BasePersonDBService.DataContext;
using BasePersonDBService.Interfaces;
using BasePersonDBService.Services;
using DSUContextDBService.DataContext;
using DSUContextDBService.Interfaces;
using DSUContextDBService.Services;
using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Office.Interop.Excel;
using Sentry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

builder.Services.AddDbContext<BASEPERSONMDFContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BasePerson"), providerOptions => providerOptions.EnableRetryOnFailure()));
builder.Services.AddDbContext<DSUContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BaseDekanat"), providerOptions => providerOptions.EnableRetryOnFailure()));
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EOR"), providerOptions => providerOptions.EnableRetryOnFailure()));

builder.Services.AddIdentity<EorDSU.Models.User, IdentityRole>(
               opts =>
               {
                   opts.Password.RequiredLength = 2;
                   opts.Password.RequireNonAlphanumeric = false;
                   opts.Password.RequireLowercase = false;
                   opts.Password.RequireUppercase = false;
                   opts.Password.RequireDigit = false;
               })
               .AddEntityFrameworkStores<ApplicationContext>();

builder.Services.AddScoped<ExcelParsingService>();

builder.Services.AddScoped<IDSUActiveData, DSUActiveData>();
builder.Services.AddScoped<IBasePersonActiveData, BasePersonActiveData>();
builder.Services.AddScoped<IApplicationActiveData, ActiveDataService>();
builder.Services.AddScoped<ISearchEntity, SearchEntityService>();

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

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<EorDSU.Models.User>>();
    var rolesManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    if (userManager.Users.ToList().Count == 0)
    {
        string adminLogin = builder.Configuration["AdminLogin"];
        string password = builder.Configuration["AdminPassword"];
        await RoleInitializer.InitializeAsync(adminLogin, password, userManager, rolesManager);
    }    
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder.AllowAnyOrigin());

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();