using EorDSU.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Discipline>()
                .HasOne(p => p.Profile)
                .WithMany(t => t.Disciplines)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FileModel>()
                .HasOne(p => p.Profile)
                .WithMany(t => t.FileModels)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FileRPD>()
                .HasOne(p => p.Discipline)
                .WithOne(t => t.FileRPD)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Discipline>()
            .HasOne(u => u.FileRPD)
            .WithOne(p => p.Discipline)
            .HasForeignKey<FileRPD>(p => p.DisciplineId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
