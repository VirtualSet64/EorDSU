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
        public CaseSDepartment? CaseSDepartment { get; set; }
    }
}
