using DSUContextDBService.Models;
using EorDSU.Models;

namespace EorDSU.ViewModels
{
    public class ResponseForDiscipline
    {
        public List<Discipline>? Disciplines { get; set; }
        public Profile? Profile { get; set; }
        public CaseSDepartment? CaseSDepartment { get; set; }
        public CaseCEdukind? CaseCEdukind { get; set; }
    }
}
