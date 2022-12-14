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
            return _basePersonContext.PersDepartments.FirstOrDefault(c => c.DepName == text);
        }

        public CaseSDepartment SearchCaseSDepartment(string text)
        {
            var sd = _dsuContext.CaseSDepartments.FirstOrDefault(c => c.DeptName == text);
            return sd;
        }

        public LevelEdu SearchLevelEdu(string text)
        {
            var levelEdu = _applicationContext.LevelEdues.FirstOrDefault(c => c.Name == text);
            if (levelEdu == null)
            {
                levelEdu = new LevelEdu(text);
                _applicationContext.LevelEdues.Add(levelEdu);
                _applicationContext.SaveChanges();
            }

            return levelEdu;
        }

        public StatusDiscipline SearchStatusDiscipline(string text)
        {
            var statusDiscipline = _applicationContext.StatusDisciplines.FirstOrDefault(c => c.Name == text);
            if (statusDiscipline == null)
            {
                statusDiscipline = new StatusDiscipline(text);
                _applicationContext.StatusDisciplines.Add(statusDiscipline);
                _applicationContext.SaveChanges();
            }

            return statusDiscipline;
        }
    }
}
