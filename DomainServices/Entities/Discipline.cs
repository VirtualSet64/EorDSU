namespace DomainServices.Entities
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
        public string? Code { get; set; }
        public FileRPD? FileRPD { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; }
        public bool IsDeletionRequest { get; set; }
    }
}
