using EorDSU.Models;

namespace EorDSU.Interface
{
    public interface ISearchEntity
    {
        public LevelEdu SearchLevelEdu(string text);
        public CaseCEdukind SearchEdukind(string text);
        public PersDepartment SearchPersDepartment(string text);
        public CaseSDepartment SearchCaseSDepartment(string text, string code);
        public StatusDiscipline SearchStatusDiscipline(string text);
    }
}
