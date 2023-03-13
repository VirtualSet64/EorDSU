using DSUContextDBService.Interfaces;
using EorDSU.Interface;
using EorDSU.Models;
using EorDSU.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Service
{
    public class SearchEntityService : ISearchEntityService
    {
        private readonly IStatusDisciplineRepository _statusDisciplineRepository;
        private readonly ILevelEduRepository _levelEduRepository;
        private readonly IDSUActiveData _dSUActiveData;
        public SearchEntityService(IStatusDisciplineRepository statusDisciplineRepository, ILevelEduRepository levelEduRepository, IDSUActiveData dSUActiveData)
        {
            _statusDisciplineRepository = statusDisciplineRepository;
            _levelEduRepository = levelEduRepository;
            _dSUActiveData = dSUActiveData;
        }

        public async Task<short?> SearchEdukind(string text)
        {
            var item = await _dSUActiveData.GetCaseCEdukinds().FirstOrDefaultAsync(c => c.Edukind == text);
            if (item != null)
                return item.EdukindId;
            return null;
        }

        public async Task<int?> SearchCaseSDepartment(string text)
        {
            var item = await _dSUActiveData.GetCaseSDepartments().FirstOrDefaultAsync(c => c.DeptName == text);
            if (item != null)
                return item.DepartmentId;
            return null;
        }

        public async Task<LevelEdu> SearchLevelEdu(string text)
        {
            var levelEdu = await _levelEduRepository.Get().FirstOrDefaultAsync(c => c.Name == text);
            if (levelEdu == null)
            {
                levelEdu = new LevelEdu(text);
                await _levelEduRepository.Create(levelEdu);
            }

            return levelEdu;
        }

        public async Task<StatusDiscipline> SearchStatusDiscipline(string text)
        {
            var statusDiscipline = await _statusDisciplineRepository.Get().FirstOrDefaultAsync(c => c.Name == text);
            if (statusDiscipline == null)
            {
                statusDiscipline = new StatusDiscipline(text);
                await _statusDisciplineRepository.Create(statusDiscipline);
            }

            return statusDiscipline;
        }
    }
}
