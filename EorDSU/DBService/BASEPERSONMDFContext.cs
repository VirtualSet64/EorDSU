using System;
using System.Collections.Generic;
using EorDSU.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EorDSU
{
    public partial class BASEPERSONMDFContext : DbContext
    {
        public DbSet<PersDepartment> PersDepartments { get; set; }
        public DbSet<PersDivision> PersDivisions { get; set; }

        public BASEPERSONMDFContext(DbContextOptions<BASEPERSONMDFContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersDepartment>(entity =>
            {
                entity.HasKey(e => e.DepId);

                entity.ToTable("pers_departments");

                entity.Property(e => e.DepId).HasColumnName("dep_id");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .HasColumnName("address");

                entity.Property(e => e.CreateYear).HasColumnName("create_year");

                entity.Property(e => e.DateAdd)
                    .HasColumnType("datetime")
                    .HasColumnName("date_add");

                entity.Property(e => e.DepAbr)
                    .HasMaxLength(10)
                    .HasColumnName("dep_abr");

                entity.Property(e => e.DepName)
                    .HasMaxLength(256)
                    .HasColumnName("dep_name");

                entity.Property(e => e.DepType).HasColumnName("dep_type");

                entity.Property(e => e.DivId).HasColumnName("div_id");

                entity.Property(e => e.ForEor).HasColumnName("for_eor");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsKaf).HasColumnName("is_kaf");

                entity.Property(e => e.IsMain).HasColumnName("is_main");

                entity.Property(e => e.UserLogin)
                    .HasMaxLength(50)
                    .HasColumnName("user_login");
            });

            modelBuilder.Entity<PersDivision>(entity =>
            {
                entity.HasKey(e => new { e.FilId, e.DivName });

                entity.ToTable("pers_divisions");

                entity.Property(e => e.FilId).HasColumnName("fil_id");

                entity.Property(e => e.DivName)
                    .HasMaxLength(256)
                    .HasColumnName("div_name");

                entity.Property(e => e.AbbrEng)
                    .HasMaxLength(50)
                    .HasColumnName("abbr_eng");

                entity.Property(e => e.CreateYear).HasColumnName("create_year");

                entity.Property(e => e.DateAdd)
                    .HasColumnType("datetime")
                    .HasColumnName("date_add");

                entity.Property(e => e.DivAbr)
                    .HasMaxLength(10)
                    .HasColumnName("div_abr");

                entity.Property(e => e.DivId).HasColumnName("div_id");

                entity.Property(e => e.ForEor).HasColumnName("for_eor");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsFaculty).HasColumnName("is_faculty");

                entity.Property(e => e.OldId).HasColumnName("old_id");

                entity.Property(e => e.OrgRate).HasColumnName("org_rate");

                entity.Property(e => e.OrgStr).HasColumnName("org_str");

                entity.Property(e => e.UserLogin)
                    .HasMaxLength(51)
                    .HasColumnName("user_login");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
