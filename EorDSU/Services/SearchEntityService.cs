using BasePersonDBService.DataContext;
using BasePersonDBService.Interfaces;
using DSUContextDBService.DataContext;
using DSUContextDBService.Interfaces;
using DSUContextDBService.Models;
using EorDSU.Common.Interfaces;
using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;
using Models;

namespace EorDSU.Service
{
    public class SearchEntityService : ISearchEntity
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IUnitOfWork _unitOfWork;
        public SearchEntityService(ApplicationContext applicationContext, IUnitOfWork unitOfWork)
        {
            _applicationContext = applicationContext;
            _unitOfWork = unitOfWork;
        }

        public CaseCEdukind SearchEdukind(string text)
        {
            return _unitOfWork.DSUActiveData.GetCaseCEdukinds().FirstOrDefault(c => c.Edukind == text);
        }

        public CaseSDepartment SearchCaseSDepartment(string text)
        {
            return _unitOfWork.DSUActiveData.GetCaseSDepartments().FirstOrDefault(c => c.DeptName == text); ;
        }

        public PersDepartment SearchPersDepartment(string text)
        {
            return _unitOfWork.BasePersonActiveData.GetPersDepartments().FirstOrDefault(c => c.DepName == text);
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
