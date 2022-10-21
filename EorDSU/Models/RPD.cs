namespace EorDSU.Models
{
    public class RPD
    {
        public int Id { get; set; }
        public int DisciplineId { get; set; }
        public int ProfileId { get; set; }
        public string? Year { get; set; }
        public string? FormEdu { get; set; } 
        public int CathedraId { get; set; }
        public int AuthorId { get; set; }
    }
}
