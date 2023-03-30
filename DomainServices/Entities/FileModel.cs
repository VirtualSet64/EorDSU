namespace DomainServices.Entities
{
    /// <summary>
    /// Файлы
    /// </summary>
    public class FileModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? OutputFileName { get; set; }
        public string? LinkToFile { get; set; }
        public int? ProfileId { get; set; }
        public Profile? Profile { get; set; }
        /// <summary>
        /// Код ЭЦП
        /// </summary>
        public string? CodeECP { get; set; } = Guid.NewGuid().ToString().ToUpper();
        public int? FileTypeId { get; set; }
        public FileType? FileType { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; }
    }
}
