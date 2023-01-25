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
        public virtual DbSet<CaseCPlat> CaseCPlats { get; set; } = null!;
        public virtual DbSet<CaseCStatus> CaseCStatuses { get; set; } = null!;
        public virtual DbSet<CaseSDepartment> CaseSDepartments { get; set; } = null!;
        public virtual DbSet<CaseSSpecialization> CaseSSpecializations { get; set; } = null!;
        public virtual DbSet<CaseSStudent> CaseSStudents { get; set; } = null!;
        public virtual DbSet<CaseSTeacher> CaseSTeachers { get; set; } = null!;
        public virtual DbSet<CaseUkoExam> CaseUkoExams { get; set; } = null!;
        public virtual DbSet<CaseUkoModule> CaseUkoModules { get; set; } = null!;
        public virtual DbSet<CaseUkoZachet> CaseUkoZachets { get; set; } = null!;

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

            modelBuilder.Entity<CaseCPlat>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("CASE_C_PLAT");

                entity.Property(e => e.PlatId).HasColumnName("PLAT_ID");

                entity.Property(e => e.PlatName)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("PLAT_NAME");
            });

            modelBuilder.Entity<CaseCStatus>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("CASE_C_STATUS");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NAME");
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
            
            modelBuilder.Entity<CaseSSpecialization>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("CASE_S_SPECIALIZATION");

                entity.Property(e => e.Code)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.Deleted).HasColumnName("DELETED");

                entity.Property(e => e.DeptId).HasColumnName("DEPT_ID");

                entity.Property(e => e.FilId).HasColumnName("FIL_ID");

                entity.Property(e => e.SpecId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("SPEC_ID");

                entity.Property(e => e.SpecName)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("SPEC_NAME");
            });

            modelBuilder.Entity<CaseSStudent>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("CASE_S_STUDENT");

                entity.Property(e => e.AbiturId).HasColumnName("ABITUR_ID");

                entity.Property(e => e.FacId).HasColumnName("FAC_ID");

                entity.Property(e => e.FilId).HasColumnName("FIL_ID");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FIRSTNAME");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LASTNAME");

                entity.Property(e => e.Nzachkn)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NZACHKN");

                entity.Property(e => e.Patr)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("PATR");

                entity.Property(e => e.Snils)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SNILS");

                entity.Property(e => e.Status).HasColumnName("STATUS");
            });
                        
            modelBuilder.Entity<CaseSTeacher>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("CASE_S_TEACHER");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("FIRSTNAME");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("LASTNAME");

                entity.Property(e => e.Patr)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("PATR");

                entity.Property(e => e.TeachId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("TEACH_ID");
            });
                        
            modelBuilder.Entity<CaseUkoExam>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("CASE_UKO_EXAM");

                entity.Property(e => e.Cathedra)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CATHEDRA");

                entity.Property(e => e.Closed).HasColumnName("CLOSED");

                entity.Property(e => e.Course).HasColumnName("COURSE");

                entity.Property(e => e.DeptId).HasColumnName("DEPT_ID");

                entity.Property(e => e.EdukindId).HasColumnName("EDUKIND_ID");

                entity.Property(e => e.FacId).HasColumnName("FAC_ID");

                entity.Property(e => e.FilId).HasColumnName("FIL_ID");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FIRSTNAME");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LASTNAME");

                entity.Property(e => e.Mark).HasColumnName("MARK");

                entity.Property(e => e.Ngroup)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("NGROUP");

                entity.Property(e => e.Patr)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("PATR");

                entity.Property(e => e.Predmet)
                    .HasMaxLength(800)
                    .IsUnicode(false)
                    .HasColumnName("PREDMET");

                entity.Property(e => e.Prepod)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("PREPOD");

                entity.Property(e => e.Rb).HasColumnName("RB");

                entity.Property(e => e.SId).HasColumnName("S_ID");

                entity.Property(e => e.SessId).HasColumnName("SESS_ID");

                entity.Property(e => e.SpecName)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("SPEC_NAME");

                entity.Property(e => e.StudentStatus).HasColumnName("STUDENT_STATUS");

                entity.Property(e => e.Subgroup)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("SUBGROUP");

                entity.Property(e => e.TeachId1).HasColumnName("TEACH_ID1");

                entity.Property(e => e.TeachId2).HasColumnName("TEACH_ID2");

                entity.Property(e => e.TeachId3).HasColumnName("TEACH_ID3");

                entity.Property(e => e.TeachId4).HasColumnName("TEACH_ID4");

                entity.Property(e => e.TeachId5).HasColumnName("TEACH_ID5");

                entity.Property(e => e.Tp).HasColumnName("TP");

                entity.Property(e => e.Veddate)
                    .HasColumnType("datetime")
                    .HasColumnName("VEDDATE");
            });

            modelBuilder.Entity<CaseUkoModule>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("CASE_UKO_MODULES");

                entity.Property(e => e.Cathedra)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CATHEDRA");

                entity.Property(e => e.Closed).HasColumnName("CLOSED");

                entity.Property(e => e.DeptId).HasColumnName("DEPT_ID");

                entity.Property(e => e.EdukindId).HasColumnName("EDUKIND_ID");

                entity.Property(e => e.FacId).HasColumnName("FAC_ID");

                entity.Property(e => e.FilId).HasColumnName("FIL_ID");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FIRSTNAME");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LASTNAME");

                entity.Property(e => e.Ngroup)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("NGROUP");

                entity.Property(e => e.Nmod).HasColumnName("NMOD");

                entity.Property(e => e.Patr)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("PATR");

                entity.Property(e => e.Predmet)
                    .HasMaxLength(800)
                    .IsUnicode(false)
                    .HasColumnName("PREDMET");

                entity.Property(e => e.Prepod)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("PREPOD");

                entity.Property(e => e.Rb).HasColumnName("RB");

                entity.Property(e => e.SId).HasColumnName("S_ID");

                entity.Property(e => e.SessId).HasColumnName("SESS_ID");

                entity.Property(e => e.SpecName)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("SPEC_NAME");

                entity.Property(e => e.StudentStatus).HasColumnName("STUDENT_STATUS");

                entity.Property(e => e.Subgroup)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("SUBGROUP");

                entity.Property(e => e.TeachId1).HasColumnName("TEACH_ID1");

                entity.Property(e => e.TeachId2).HasColumnName("TEACH_ID2");

                entity.Property(e => e.TeachId3).HasColumnName("TEACH_ID3");

                entity.Property(e => e.Veddate)
                    .HasColumnType("datetime")
                    .HasColumnName("VEDDATE");
            });

            modelBuilder.Entity<CaseUkoZachet>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("CASE_UKO_ZACHET");

                entity.Property(e => e.Cathedra)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CATHEDRA");

                entity.Property(e => e.Closed).HasColumnName("CLOSED");

                entity.Property(e => e.Course).HasColumnName("COURSE");

                entity.Property(e => e.DeptId).HasColumnName("DEPT_ID");

                entity.Property(e => e.EdukindId).HasColumnName("EDUKIND_ID");

                entity.Property(e => e.FacId).HasColumnName("FAC_ID");

                entity.Property(e => e.FilId).HasColumnName("FIL_ID");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FIRSTNAME");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LASTNAME");

                entity.Property(e => e.Mark).HasColumnName("MARK");

                entity.Property(e => e.Ngroup)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("NGROUP");

                entity.Property(e => e.Patr)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("PATR");

                entity.Property(e => e.Predmet)
                    .HasMaxLength(800)
                    .IsUnicode(false)
                    .HasColumnName("PREDMET");

                entity.Property(e => e.Prepod)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("PREPOD");

                entity.Property(e => e.Rb).HasColumnName("RB");

                entity.Property(e => e.SId).HasColumnName("S_ID");

                entity.Property(e => e.SessId).HasColumnName("SESS_ID");

                entity.Property(e => e.SpecName)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("SPEC_NAME");

                entity.Property(e => e.StudentStatus).HasColumnName("STUDENT_STATUS");

                entity.Property(e => e.Subgroup)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("SUBGROUP");

                entity.Property(e => e.TeachId1).HasColumnName("TEACH_ID1");

                entity.Property(e => e.TeachId2).HasColumnName("TEACH_ID2");

                entity.Property(e => e.TeachId3).HasColumnName("TEACH_ID3");

                entity.Property(e => e.TeachId4).HasColumnName("TEACH_ID4");

                entity.Property(e => e.TeachId5).HasColumnName("TEACH_ID5");

                entity.Property(e => e.Tp).HasColumnName("TP");

                entity.Property(e => e.Veddate)
                    .HasColumnType("datetime")
                    .HasColumnName("VEDDATE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
