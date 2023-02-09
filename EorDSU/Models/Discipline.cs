namespace EorDSU.Models
{
    /// <summary>
    /// Дисциплина
    /// </summary>
    public class Discipline
    {
        public int Id { get; set; }
        public string? DisciplineName { get; set; }        
        public int? ProfileId { get; set; }
        public Profile? Profile { get; set; } = new Profile();
        public int? StatusDisciplineId { get; set; }
        public StatusDiscipline? StatusDiscipline { get; set; } = new StatusDiscipline();
        public string? Code { get; set; }
        public FileRPD? FileRPD { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
