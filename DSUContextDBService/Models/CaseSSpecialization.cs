namespace DSUContextDBService.Models
{
    public partial class CaseSSpecialization
    {
        public int SpecId { get; set; }
        public string? SpecName { get; set; }
        public int? DeptId { get; set; }
        public string? Code { get; set; }
        public bool Deleted { get; set; }
        public int FilId { get; set; }
        public bool P { get; set; }
    }
}
