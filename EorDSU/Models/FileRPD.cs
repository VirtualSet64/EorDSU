using Models;

namespace EorDSU.Models
{
    public class FileRPD
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? PersonId { get; set; }
        public Person? Person { get; set; }
        public int? DisciplineId { get; set; }
        public Discipline? Discipline { get; set; }
        /// <summary>
        /// Код ЭЦП
        /// </summary>
        public string? CodeECP { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
