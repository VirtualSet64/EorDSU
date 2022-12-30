namespace EorDSU.Models
{
    public class StatusDiscipline
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsDeleted { get; set; }

        public StatusDiscipline(string name)
        {
            Name = name;
        }

        public StatusDiscipline()
        {
        }
    }
}
