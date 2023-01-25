namespace Models
{
    public partial class PersDivision
    {
        public int DivId { get; set; }
        public int FilId { get; set; }
        public string DivName { get; set; } = null!;
        public string? DivAbr { get; set; }
        public int? CreateYear { get; set; }
        public DateTime? DateAdd { get; set; }
        public string? UserLogin { get; set; }
        public int? IsFaculty { get; set; }
        public int? IsActive { get; set; }
        public int? OrgStr { get; set; }
        public int? OrgRate { get; set; }
        public int? ForEor { get; set; }
        public string? AbbrEng { get; set; }
        public int? OldId { get; set; }
    }
}
