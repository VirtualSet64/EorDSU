namespace DSUContextDBService.Models
{
    public partial class CaseSStudent
    {
        public int Id { get; set; }
        public int FilId { get; set; }
        public int FacId { get; set; }
        public string? Lastname { get; set; }
        public string? Firstname { get; set; }
        public string? Patr { get; set; }
        public short Status { get; set; }
        public string? Nzachkn { get; set; }
        public string? Snils { get; set; }
        public int? AbiturId { get; set; }
    }
}
