using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;

namespace EorDSU.Repository
{
    public class SearchEntityRepository : ISearchEntity
    {
        private readonly DSUContext _dsuContext;
        private readonly ApplicationContext _applicationContext;
        private readonly BASEPERSONMDFContext _basePersonContext;
        public SearchEntityRepository(DSUContext dsuContext, ApplicationContext applicationContext, BASEPERSONMDFContext basePersonContext)
        {
            _dsuContext = dsuContext;
            _applicationContext = applicationContext;
            _basePersonContext = basePersonContext;
        }

        public CaseCEdukind SearchEdukind(string text)
        {
            return _dsuContext.CaseCEdukinds.FirstOrDefault(c => c.Edukind == text);
        }

        public PersDepartment SearchPersDepartment(string text)
        {
            var sd = _basePersonContext.PersDepartments.FirstOrDefault(c => c.DepName == text);
            return sd;
        }

        public CaseSDepartment SearchCaseSDepartment(string text)
        {
            var sd = _dsuContext.CaseSDepartments.FirstOrDefault(c => c.DeptName == text);
            return sd;
        }

        public LevelEdu SearchLevelEdu(string text)
        {
            var levelEdu = _applicationContext.LevelEdues.FirstOrDefault(c => c.Name == text);
            levelEdu ??= new LevelEdu(text);
            return levelEdu;
        }

        public StatusDiscipline SearchStatusDiscipline(string text)
        {
            var statusDiscipline = _applicationContext.StatusDisciplines.FirstOrDefault(c => c.Name == text);
            statusDiscipline ??= new StatusDiscipline(text);
            return statusDiscipline;
        }
    }
}
