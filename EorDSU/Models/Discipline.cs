namespace EorDSU.Models
{
    /// <summary>
    /// Дисциплина
    /// </summary>
    public class Discipline
    {
        public int Id { get; set; }
        public string? DisciplineName { get; set; }
        /// <summary>
        /// Направление обучения
        /// </summary>
        public int? CaseSDepartmentId { get; set; }
        public CaseSDepartment? CaseSDepartment { get; set; }
    }
}
