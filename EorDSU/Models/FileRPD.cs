namespace EorDSU.Models
{
    public class FileRPD
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? DisciplineId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
