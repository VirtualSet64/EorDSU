namespace EorDSU.Models
{
    /// <summary>
    /// Файлы
    /// </summary>
    public class FileModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Year { get; set; }
        public int? ProfileId { get; set; }
        public Profile? Profile { get; set; }
        public FileType? Type { get; set; }
    }
}
