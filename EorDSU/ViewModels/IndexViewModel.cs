using EorDSU.Models;

namespace EorDSU.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<FileModel>? FileModels { get; set; }
        public IEnumerable<Profile>? Profiles { get; set; }
        public IEnumerable<CaseSDepartment>? CaseSDepartments { get; set; }
    }
}
