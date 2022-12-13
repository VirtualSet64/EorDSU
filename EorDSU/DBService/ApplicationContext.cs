using EorDSU.Models;
using EorDSU.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using FileModel = EorDSU.Models.FileModel;

namespace EorDSU.DBService
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Discipline> Disciplines { get; set; } = null!;
        public DbSet<Profile> Profiles { get; set; } = null!;
        public DbSet<FileModel> FileModels { get; set; } = null!;
        public DbSet<FileRPD> FileRPDs { get; set; } = null!;
        public DbSet<LevelEdu> LevelEdues { get; set; } = null!;
        public DbSet<StatusDiscipline> StatusDisciplines { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();   // удаляем базу данных при первом обращении
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
    }
}
