using EorDSU.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.DBService
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<RPD> RPDs { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
    }
}
