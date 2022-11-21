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
        public Profile? Profile { get; set; }
        public int? StatusDisciplineId { get; set; }
        public StatusDiscipline? StatusDiscipline { get; set; }
        public int? FileRPDId { get; set; }
        public FileRPD? FileRPD { get; set; }
    }
}
