namespace EorDSU.Models
{
    public class RPD
    {
        public int Id { get; set; }
        public int? PersDepartmentId { get; set; }
        public PersDepartment? PersDepartment { get; set; }
        public int? DisciplineId { get; set; }
        public Discipline? Discipline { get; set; }
        public List<Profile>? ListProfile { get; set; }
        public string? Year { get; set; }
        public short? EdukindId { get; set; }
        public CaseCEdukind? Edukind { get; set; }
        public int? AuthorId { get; set; }
        public Person? Author { get; set; }
        public int LevelEduId { get; set; }
        public LevelEdu? LevelEdu { get; set; }
    }
}
