using EorDSU.Common.Interfaces;
using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<short?> SearchEdukind(string text)
        {
            var item = await _unitOfWork.DSUActiveData.GetCaseCEdukinds().FirstOrDefaultAsync(c => c.Edukind == text);
            if (item != null)
                return item.EdukindId;
            return null;
        }

        public async Task<int?> SearchCaseSDepartment(string text)
        {
            var item = await _unitOfWork.DSUActiveData.GetCaseSDepartments().FirstOrDefaultAsync(c => c.DeptName == text);
            if (item != null)
                return item.DepartmentId;
            return null;
        }

        public async Task<int?> SearchPersDepartment(string text)
        {
            var item = await _unitOfWork.BasePersonActiveData.GetPersDepartments().FirstOrDefaultAsync(c => c.DepName == text);
            if (item != null)
                return item.DepId;
            return null;
        }

        public async Task<LevelEdu> SearchLevelEdu(string text)
        {
            var levelEdu = await _applicationContext.LevelEdues.FirstOrDefaultAsync(c => c.Name == text);
            if (levelEdu == null)
            {
                levelEdu = new LevelEdu(text);
                _applicationContext.LevelEdues.Add(levelEdu);
                _applicationContext.SaveChanges();
            }

            return levelEdu;
        }

        public async Task<StatusDiscipline> SearchStatusDiscipline(string text)
        {
            var statusDiscipline = await _applicationContext.StatusDisciplines.FirstOrDefaultAsync(c => c.Name == text);
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
