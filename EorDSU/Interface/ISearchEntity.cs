using EorDSU.Models;

namespace EorDSU.Interface
{
    public interface ISearchEntity
    {
        public LevelEdu SearchLevelEdu(string text);
        public CaseCEdukind SearchEdukind(string text);
        public Discipline SearchDiscipline(string text);
        public PersDepartment SearchPersDepartment(string text);
        public Profile SearchProfile(string text);
        //public StatusDiscipline SearchStatusDiscipline(string text);
    }
}
