namespace DSUContextDBService.Models
{
    public partial class CaseCFaculty
    {
        public int FacId { get; set; }
        public string? FacName { get; set; }
        public string? Abr { get; set; }
        public bool Deleted { get; set; }
        public bool College { get; set; }
    }
}
