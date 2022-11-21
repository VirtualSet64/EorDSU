using EorDSU.DBService;
using EorDSU.Models;
using EorDSU.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer("DefaultConnection"));
//builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer("DSU"));



builder.Services.AddIdentity<User, IdentityRole>(opts =>
{
    opts.Password.RequiredLength = 0;   // ����������� �����
    opts.Password.RequireNonAlphanumeric = false;   // ��������� �� �� ���������-�������� �������
    opts.Password.RequireLowercase = false; // ��������� �� ������� � ������ ��������
    opts.Password.RequireUppercase = false; // ��������� �� ������� � ������� ��������
    opts.Password.RequireDigit = false; // ��������� �� �����
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
