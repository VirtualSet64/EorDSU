using System;
using System.Collections.Generic;
using EorDSU.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EorDSU.DBService
{
    public class DSUContext : DbContext
    {
        public DSUContext(DbContextOptions<DSUContext> options)
            : base(options)
        {
        }

        public DbSet<CaseCEdukind> CaseCEdukinds { get; set; } = null!;
        public DbSet<CaseSDepartment> CaseSDepartments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CaseCEdukind>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("CASE_C_EDUKIND");

                entity.Property(e => e.Abr)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ABR");

                entity.Property(e => e.Edukind)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("EDUKIND");

                entity.Property(e => e.EdukindId).HasColumnName("EDUKIND_ID");

                entity.Property(e => e.Yearedu).HasColumnName("YEAREDU");
            });

            modelBuilder.Entity<CaseSDepartment>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("CASE_S_DEPARTMENT");

                entity.Property(e => e.Abr)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ABR");

                entity.Property(e => e.Code)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.Deleted).HasColumnName("DELETED");

                entity.Property(e => e.DepartmentId).HasColumnName("DEPARTMENT_ID");

                entity.Property(e => e.DeptName)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("DEPT_NAME");

                entity.Property(e => e.FacId).HasColumnName("FAC_ID");

                entity.Property(e => e.Godequalif)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("GODEQUALIF");

                entity.Property(e => e.Qualification)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("QUALIFICATION");
            });
        }
    }
}
