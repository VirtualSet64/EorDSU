using System;
using System.Collections.Generic;

namespace SvedenOop.eor
{
    public partial class ViewUmkForKaf
    {
        public int Id { get; set; }
        public int? PersonId { get; set; }
        public string? Author { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public string? Name { get; set; }
        public string? FileName { get; set; }
        public string? Title { get; set; }
        public string? NaprName { get; set; }
        public string? DeptCode { get; set; }
        public string? DisciplineName { get; set; }
        public int? CathId { get; set; }
        public string? Fio { get; set; }
    }
}
