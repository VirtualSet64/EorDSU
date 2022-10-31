using EorDSU.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FileModel = EorDSU.Models.FileModel;

namespace EorDSU.DBService
{
    public class ApplicationContext : DbContext//IdentityDbContext<User>
    {
        public DbSet<Discipline> Disciplines { get; set; } = null!;
        public DbSet<Profile> Profiles { get; set; } = null!;
        public DbSet<RPD> RPDs { get; set; } = null!;
        public DbSet<FileModel> FileModels { get; set; } = null!;
        public DbSet<LevelEdu> LevelEdues { get; set; } = null!;
        public DbSet<User> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
    }
}
