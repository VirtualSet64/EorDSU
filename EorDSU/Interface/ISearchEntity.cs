using DSUContextDBService.Models;
using EorDSU.Models;
using Models;

namespace EorDSU.Interface
{
    public interface ISearchEntity
    {       
        public CaseCEdukind SearchEdukind(string text);        
        public CaseSDepartment SearchCaseSDepartment(string text);
        public PersDepartment SearchPersDepartment(string text);
        public StatusDiscipline SearchStatusDiscipline(string text);
        public LevelEdu SearchLevelEdu(string text);
    }
}
