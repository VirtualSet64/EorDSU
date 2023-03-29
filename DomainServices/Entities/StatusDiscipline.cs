namespace DomainServices.Entities
{
    public class StatusDiscipline
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsDeletionRequest { get; set; }
        public List<Discipline>? Disciplines { get; set; }

        public StatusDiscipline(string name)
        {
            Name = name;
        }

        public StatusDiscipline()
        {
        }
    }
}
