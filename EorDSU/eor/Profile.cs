using System;
using System.Collections.Generic;

namespace EorDSU.eor
{
    public partial class Profile
    {
        public Profile()
        {
            Disciplines = new HashSet<Discipline>();
            Umks = new HashSet<Umk>();
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Abr { get; set; }
        public int DeptId { get; set; }
        public int? DisciplineCount { get; set; }

        public virtual ICollection<Discipline> Disciplines { get; set; }
        public virtual ICollection<Umk> Umks { get; set; }
    }
}
