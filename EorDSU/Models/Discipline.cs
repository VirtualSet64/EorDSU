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
        public virtual Profile? Profile { get; set; }
        public int? StatusDisciplineId { get; set; }
        public virtual StatusDiscipline? StatusDiscipline { get; set; }
        public int? FileRPDId { get; set; }
        public virtual FileRPD? FileRPD { get; set; }
    }
}
