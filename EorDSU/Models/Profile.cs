namespace EorDSU.Models
{
    /// <summary>
    /// Профиль обучения
    /// </summary>
    public class Profile
    {
        public int Id { get; set; } 
        public string? ProfileName { get; set; }
        public int? Year { get; set; }
        public List<CaseSDepartment>? ListCaseSDepartment { get; set; }
    }
}
