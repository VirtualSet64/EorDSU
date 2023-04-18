namespace DSUContextDBService.Models
{
    public partial class CaseSDepartment
    {
        public int DepartmentId { get; set; }
        public int FacId { get; set; }
        public string DeptName { get; set; }
        public string? Abr { get; set; }
        public string? Code { get; set; }
        public string? Qualification { get; set; }
        public bool Deleted { get; set; }
    }
}
