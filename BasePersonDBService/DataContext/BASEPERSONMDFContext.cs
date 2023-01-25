using Microsoft.EntityFrameworkCore;
using Models;

namespace BasePersonDBService.DataContext
{
    public partial class BASEPERSONMDFContext : DbContext
    {
        public BASEPERSONMDFContext()
        {
        }

        public BASEPERSONMDFContext(DbContextOptions<BASEPERSONMDFContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PersDepartment> PersDepartments { get; set; } = null!;
        public virtual DbSet<PersDivision> PersDivisions { get; set; } = null!;
        public virtual DbSet<PersFilial> PersFilials { get; set; } = null!;
        public virtual DbSet<Person> People { get; set; } = null!;

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

            modelBuilder.Entity<PersFilial>(entity =>
            {
                entity.HasKey(e => e.FilName);

                entity.ToTable("pers_filials");

                entity.Property(e => e.FilName)
                    .HasMaxLength(100)
                    .HasColumnName("fil_name");

                entity.Property(e => e.CreateYear)
                    .HasColumnType("date")
                    .HasColumnName("create_year");

                entity.Property(e => e.DateAdd)
                    .HasColumnType("datetime")
                    .HasColumnName("date_add");

                entity.Property(e => e.FilAbr)
                    .HasMaxLength(10)
                    .HasColumnName("fil_abr");

                entity.Property(e => e.FilId).HasColumnName("fil_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.OldId).HasColumnName("old_id");

                entity.Property(e => e.UserLogin)
                    .HasMaxLength(50)
                    .HasColumnName("user_login");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("person");

                entity.Property(e => e.PersonId).HasColumnName("person_id");

                entity.Property(e => e.AdditionalInfo)
                    .HasMaxLength(2048)
                    .HasColumnName("additional_info");

                entity.Property(e => e.Birthdate)
                    .HasColumnType("date")
                    .HasColumnName("birthdate");

                entity.Property(e => e.Birthplace)
                    .HasMaxLength(256)
                    .HasColumnName("birthplace");

                entity.Property(e => e.BirthplaceOkato)
                    .HasMaxLength(20)
                    .HasColumnName("birthplace_okato");

                entity.Property(e => e.CitizenshipOkin)
                    .HasMaxLength(5)
                    .HasColumnName("citizenship_okin");

                entity.Property(e => e.DateInsert)
                    .HasColumnType("datetime")
                    .HasColumnName("date_insert");

                entity.Property(e => e.DatePriem)
                    .HasColumnType("datetime")
                    .HasColumnName("date_priem");

                entity.Property(e => e.FolderId).HasColumnName("folder_id");

                entity.Property(e => e.IKafPersonId).HasColumnName("i_kaf_personID");

                entity.Property(e => e.Inn)
                    .HasMaxLength(20)
                    .HasColumnName("inn");

                entity.Property(e => e.IsLeave).HasColumnName("is_leave");

                entity.Property(e => e.KafLogin)
                    .HasMaxLength(50)
                    .HasColumnName("kaf_login");

                entity.Property(e => e.KafPassword)
                    .HasMaxLength(50)
                    .HasColumnName("kaf_password");

                entity.Property(e => e.Kdol)
                    .HasMaxLength(10)
                    .HasColumnName("KDOL");

                entity.Property(e => e.Login)
                    .HasMaxLength(50)
                    .HasColumnName("login");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.NationalityOkin)
                    .HasMaxLength(5)
                    .HasColumnName("nationality_okin");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasColumnName("order_date");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.Patronymic)
                    .HasMaxLength(50)
                    .HasColumnName("patronymic");

                entity.Property(e => e.Photo)
                    .HasMaxLength(50)
                    .HasColumnName("photo");

                entity.Property(e => e.Sex)
                    .HasMaxLength(3)
                    .HasColumnName("sex");

                entity.Property(e => e.StrSvid)
                    .HasMaxLength(20)
                    .HasColumnName("str_svid");

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .HasColumnName("surname");

                entity.Property(e => e.TabN)
                    .HasMaxLength(8)
                    .HasColumnName("tab_N");

                entity.Property(e => e.UserLogin)
                    .HasMaxLength(50)
                    .HasColumnName("user_login");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
