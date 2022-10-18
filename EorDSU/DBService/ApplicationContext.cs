using EorDSU.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.DBService
{
    public class ApplicationContext : DbContext
    {


        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
    }
}
