using System;
using System.Collections.Generic;

namespace EorDSU.eor
{
    public partial class Discipline
    {
        public Discipline()
        {
            Umks = new HashSet<Umk>();
            Educators = new HashSet<Educator>();
        }

        public int Id { get; set; }
        public string? DisciplineName { get; set; }
        public string? Link { get; set; }
        public int Position { get; set; }
        public int StatusId { get; set; }
        public int DeptId { get; set; }
        public string? DeptCode { get; set; }
        public int SubPosition { get; set; }
        public DateTime DateCreate { get; set; }
        public int? ProfileId { get; set; }

        public virtual Profile? Profile { get; set; }
        public virtual Status Status { get; set; } = null!;
        public virtual ICollection<Umk> Umks { get; set; }

        public virtual ICollection<Educator> Educators { get; set; }
    }
}
