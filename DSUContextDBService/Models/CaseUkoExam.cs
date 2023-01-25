namespace DSUContextDBService.Models
{
    public partial class CaseUkoExam
    {
        public int Id { get; set; }
        public short StudentStatus { get; set; }
        public int FilId { get; set; }
        public int FacId { get; set; }
        public int DeptId { get; set; }
        public int EdukindId { get; set; }
        public int SessId { get; set; }
        public string Ngroup { get; set; } = null!;
        public string? SpecName { get; set; }
        public string? Subgroup { get; set; }
        public string? Prepod { get; set; }
        public int Tp { get; set; }
        public string? Lastname { get; set; }
        public string? Firstname { get; set; }
        public string? Patr { get; set; }
        public short? Course { get; set; }
        public int Mark { get; set; }
        public short Rb { get; set; }
        public int SId { get; set; }
        public string? Predmet { get; set; }
        public string? Cathedra { get; set; }
        public DateTime Veddate { get; set; }
        public int N { get; set; }
        public bool Closed { get; set; }
        public int TeachId1 { get; set; }
        public int TeachId2 { get; set; }
        public int TeachId3 { get; set; }
        public int TeachId4 { get; set; }
        public int TeachId5 { get; set; }
    }
}
