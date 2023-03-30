using System;
using System.Collections.Generic;

namespace SvedenOop.eor
{
    public partial class Napr
    {
        public Napr()
        {
            Umks = new HashSet<Umk>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Umk> Umks { get; set; }
    }
}
