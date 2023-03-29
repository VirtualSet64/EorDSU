using System;
using System.Collections.Generic;

namespace EorDSU.eor
{
    public partial class Umk
    {
        public Umk()
        {
            Educators = new HashSet<Educator>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Semester { get; set; }
        public string? Author { get; set; }
        public int? NaprId { get; set; }
        public int TypeId { get; set; }
        public string? FileName { get; set; }
        public bool Deleted { get; set; }
        public DateTime Date { get; set; }
        public int FacId { get; set; }
        public int? CathId { get; set; }
        public int? DeptId { get; set; }
        public string? ProfileStr { get; set; }
        public string? Code { get; set; }
        public int? StatusId { get; set; }
        public int? ProfileId { get; set; }
        public int? DisciplineId { get; set; }

        public virtual Discipline? Discipline { get; set; }
        public virtual Napr? Napr { get; set; }
        public virtual Profile? Profile { get; set; }
        public virtual Status? Status { get; set; }
        public virtual TypeUmk Type { get; set; } = null!;

        public virtual ICollection<Educator> Educators { get; set; }
    }
}
