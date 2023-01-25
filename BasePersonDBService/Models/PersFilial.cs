namespace Models
{
    public partial class PersFilial
    {
        public int FilId { get; set; }
        public string FilName { get; set; } = null!;
        public string? FilAbr { get; set; }
        public DateTime? CreateYear { get; set; }
        public DateTime? DateAdd { get; set; }
        public string? UserLogin { get; set; }
        public int? IsActive { get; set; }
        public int? OldId { get; set; }
    }
}
