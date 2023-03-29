using System;
using System.Collections.Generic;

namespace EorDSU.eor
{
    public partial class TypeUmk
    {
        public TypeUmk()
        {
            Umks = new HashSet<Umk>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Umk> Umks { get; set; }
    }
}
