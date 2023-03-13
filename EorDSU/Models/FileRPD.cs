using Models;

namespace EorDSU.Models
{
    /// <summary>
    /// Файлы РПД
    /// </summary>
    public class FileRPD
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        /// <summary>
        /// Автор РПД
        /// </summary>
        public int? PersonId { get; set; }
        public int? DisciplineId { get; set; }
        public Discipline? Discipline { get; set; }
        /// <summary>
        /// Код ЭЦП
        /// </summary>
        public string? CodeECP { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; }
    }
}
