using EorDSU.Models;

namespace EorDSU.ResponseModel
{
    public class DataResponseForAll
    {
        public List<PersDivision>? PersDivision { get; set; }
        public List<CaseSDepartment>? CaseSDepartments { get; set; }
        public List<int>? Years { get; set; }
        public List<FileModel>? FileModels { get; set; }
        public List<Profile>? Profiles { get; set; }
        public List<CaseCEdukind>? CaseCEdukinds { get; set; }
        public string? SrokDeystvGosAccred { get; set; }
        public List<LevelEdu>? LevelEdus { get; set; }
    }
}
