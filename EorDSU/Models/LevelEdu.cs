namespace EorDSU.Models
{
    /// <summary>
    /// Уровень образования
    /// </summary>
    public class LevelEdu
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsDeleted { get; set; }

        public LevelEdu(string name)
        {
            Name = name;
        }
        public LevelEdu()
        {
        }
    }
}
