using System;
using System.Collections.Generic;

namespace EorDSU.eor
{
    public partial class Status
    {
        public Status()
        {
            Disciplines = new HashSet<Discipline>();
            Umks = new HashSet<Umk>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Abr { get; set; }

        public virtual ICollection<Discipline> Disciplines { get; set; }
        public virtual ICollection<Umk> Umks { get; set; }
    }
}
