namespace EorDSU.Models
{
    /// <summary>
    /// Профиль обучения
    /// </summary>
    public class Profile
    {
        public int Id { get; set; } 
        public string? ProfileName { get; set; }
        /// <summary>
        /// Срок обучения
        /// </summary>
        public int? TermEdu { get; set; }
        public int? Year { get; set; }
        /// <summary>
        /// Ссылка на страницу с результатами приема
        /// </summary>
        public string? LinkToPriemResult { get; set; }
        /// <summary>
        /// Ссылка на страницу с предметами
        /// </summary>
        public string? LinkToPRD { get; set; }
        public int? CaseSDepartmentId { get; set; }
        public CaseSDepartment? CaseSDepartment { get; set; }
        public int? PersDepartmentId { get; set; }
        public PersDepartment? PersDepartment { get; set; }
    }
}
