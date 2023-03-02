using DSUContextDBService.Models;
using EorDSU.Models;

namespace EorDSU.ViewModels
{
    public class DataForTableResponse
    {
        public Profile? Profile { get; set; }
        public CaseSDepartment? CaseSDepartment { get; set; }
        public CaseCEdukind? CaseCEdukind { get; set; }
        public List<Discipline>? Practics { get; set; }
    }
}
