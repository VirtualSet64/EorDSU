namespace EorDSU.Models
{
    public class Files
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DisciplineId { get; set; }
        public int ProfileId { get; set; }
        public int TeacherId { get; set; }
        public int AuthorId { get; set; }
    }
}
