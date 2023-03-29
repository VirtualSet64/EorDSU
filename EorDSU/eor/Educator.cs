using System;
using System.Collections.Generic;

namespace EorDSU.eor
{
    public partial class Educator
    {
        public Educator()
        {
            Disciplines = new HashSet<Discipline>();
            Umks = new HashSet<Umk>();
        }

        public int Id { get; set; }
        public int PersonId { get; set; }
        public string? Fio { get; set; }

        public virtual ICollection<Discipline> Disciplines { get; set; }
        public virtual ICollection<Umk> Umks { get; set; }
    }
}
