using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EorDSU.eor
{
    public partial class EORContext : DbContext
    {
        public EORContext()
        {
        }

        public EORContext(DbContextOptions<EORContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Discipline> Disciplines { get; set; } = null!;
        public virtual DbSet<Educator> Educators { get; set; } = null!;
        public virtual DbSet<LecturesLink> LecturesLinks { get; set; } = null!;
        public virtual DbSet<MigrationHistory> MigrationHistories { get; set; } = null!;
        public virtual DbSet<Napr> Naprs { get; set; } = null!;
        public virtual DbSet<Profile> Profiles { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<TypeUmk> TypeUmks { get; set; } = null!;
        public virtual DbSet<Umk> Umks { get; set; } = null!;
        public virtual DbSet<ViewUmkForKaf> ViewUmkForKafs { get; set; } = null!;
        public virtual DbSet<VwEor> VwEors { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Discipline>(entity =>
            {
                entity.HasIndex(e => e.ProfileId, "IX_Profile1_Id");

                entity.HasIndex(e => e.StatusId, "IX_StatusId");

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.ProfileId).HasColumnName("Profile_Id");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Disciplines)
                    .HasForeignKey(d => d.ProfileId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_dbo.HelpNaprs_dbo.Profiles_Profile1_Id");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Disciplines)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_dbo.HelpNaprs_dbo.Status_StatusId");
            });

            modelBuilder.Entity<Educator>(entity =>
            {
                entity.Property(e => e.Fio).HasColumnName("FIO");

                entity.HasMany(d => d.Disciplines)
                    .WithMany(p => p.Educators)
                    .UsingEntity<Dictionary<string, object>>(
                        "EducatorDiscipline",
                        l => l.HasOne<Discipline>().WithMany().HasForeignKey("DisciplineId").HasConstraintName("FK_dbo.EducatorDisciplines_dbo.Disciplines_Discipline_Id"),
                        r => r.HasOne<Educator>().WithMany().HasForeignKey("EducatorId").HasConstraintName("FK_dbo.EducatorDisciplines_dbo.Educators_Educator_Id"),
                        j =>
                        {
                            j.HasKey("EducatorId", "DisciplineId").HasName("PK_dbo.EducatorDisciplines");

                            j.ToTable("EducatorDisciplines");

                            j.HasIndex(new[] { "DisciplineId" }, "IX_Discipline_Id");

                            j.HasIndex(new[] { "EducatorId" }, "IX_Educator_Id");

                            j.IndexerProperty<int>("EducatorId").HasColumnName("Educator_Id");

                            j.IndexerProperty<int>("DisciplineId").HasColumnName("Discipline_Id");
                        });

                entity.HasMany(d => d.Umks)
                    .WithMany(p => p.Educators)
                    .UsingEntity<Dictionary<string, object>>(
                        "EducatorUmk",
                        l => l.HasOne<Umk>().WithMany().HasForeignKey("UmkId").HasConstraintName("FK_dbo.EducatorUMKs_dbo.UMKs_UMK_Id"),
                        r => r.HasOne<Educator>().WithMany().HasForeignKey("EducatorId").HasConstraintName("FK_dbo.EducatorUMKs_dbo.Educators_Educator_Id"),
                        j =>
                        {
                            j.HasKey("EducatorId", "UmkId").HasName("PK_dbo.EducatorUMKs");

                            j.ToTable("EducatorUMKs");

                            j.HasIndex(new[] { "EducatorId" }, "IX_Educator_Id");

                            j.HasIndex(new[] { "UmkId" }, "IX_UMK_Id");

                            j.IndexerProperty<int>("EducatorId").HasColumnName("Educator_Id");

                            j.IndexerProperty<int>("UmkId").HasColumnName("UMK_Id");
                        });
            });

            modelBuilder.Entity<MigrationHistory>(entity =>
            {
                entity.HasKey(e => e.MigrationId)
                    .HasName("PK_dbo.__MigrationHistory");

                entity.ToTable("__MigrationHistory");

                entity.Property(e => e.MigrationId).HasMaxLength(255);

                entity.Property(e => e.ProductVersion).HasMaxLength(32);
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.Property(e => e.Abr).HasColumnName("ABR");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.Abr).HasColumnName("ABR");
            });

            modelBuilder.Entity<TypeUmk>(entity =>
            {
                entity.ToTable("TypeUMKs");
            });

            modelBuilder.Entity<Umk>(entity =>
            {
                entity.ToTable("UMKs");

                entity.HasIndex(e => e.DisciplineId, "IX_DisciplineId");

                entity.HasIndex(e => e.NaprId, "IX_NaprId");

                entity.HasIndex(e => e.ProfileId, "IX_ProfileId");

                entity.HasIndex(e => e.StatusId, "IX_StatusId");

                entity.HasIndex(e => e.TypeId, "IX_TypeId");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.Discipline)
                    .WithMany(p => p.Umks)
                    .HasForeignKey(d => d.DisciplineId)
                    .HasConstraintName("FK_dbo.UMKs_dbo.Disciplines_DisciplineId");

                entity.HasOne(d => d.Napr)
                    .WithMany(p => p.Umks)
                    .HasForeignKey(d => d.NaprId)
                    .HasConstraintName("FK_dbo.UMKs_dbo.Naprs_NaprId");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Umks)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK_dbo.UMKs_dbo.Profiles_ProfileId");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Umks)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_dbo.UMKs_dbo.Status_StatusId");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Umks)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_dbo.UMKs_dbo.TypeUMKs_TypeId");
            });

            modelBuilder.Entity<ViewUmkForKaf>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewUmkForKaf");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Fio).HasColumnName("FIO");

                entity.Property(e => e.NaprName).HasColumnName("NAPR_Name");
            });

            modelBuilder.Entity<VwEor>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Vw_Eor");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
