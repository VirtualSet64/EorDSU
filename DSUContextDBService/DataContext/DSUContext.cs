using Microsoft.EntityFrameworkCore;
using DSUContextDBService.Models;

namespace DSUContextDBService.DataContext
{
    public partial class DSUContext : DbContext
    {
        public DSUContext()
        {
        }

        public DSUContext(DbContextOptions<DSUContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CaseCEdue> CaseCEdues { get; set; } = null!;
        public virtual DbSet<CaseCEdukind> CaseCEdukinds { get; set; } = null!;
        public virtual DbSet<CaseSDepartment> CaseSDepartments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CaseCEdue>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("CASE_C_EDUES");

                entity.Property(e => e.EduesId).HasColumnName("EDUES_ID");

                entity.Property(e => e.EduesName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EDUES_NAME");
            });

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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
