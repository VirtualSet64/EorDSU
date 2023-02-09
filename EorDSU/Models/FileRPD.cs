namespace EorDSU.Models
{
    public class FileRPD
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? DisciplineId { get; set; }
        public Discipline? Discipline { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
