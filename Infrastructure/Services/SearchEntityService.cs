using DSUContextDBService.Interfaces;
using Ifrastructure.Services.Interface;
using DomainServices.Entities;
using Ifrastructure.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace Ifrastructure.Service
{
    public class SearchEntityService : ISearchEntityService
    {
        private readonly IStatusDisciplineRepository _statusDisciplineRepository;
        private readonly IFileTypeRepository _fileTypeRepository;
        private readonly ILevelEduRepository _levelEduRepository;
        private readonly IDSUActiveData _dSUActiveData;
        public SearchEntityService(IStatusDisciplineRepository statusDisciplineRepository, ILevelEduRepository levelEduRepository, IDSUActiveData dSUActiveData, IFileTypeRepository fileTypeRepository)
        {
            _statusDisciplineRepository = statusDisciplineRepository;
            _levelEduRepository = levelEduRepository;
            _dSUActiveData = dSUActiveData;
            _fileTypeRepository = fileTypeRepository;
        }

        public async Task<int?> SearchEdukind(string text)
        {
            var item = await _dSUActiveData.GetCaseCEdukinds().FirstOrDefaultAsync(c => c.Edukind.ToLower() == text.ToLower());
            if (item != null)
                return item.EdukindId;
            return null;
        }

        public async Task<int?> SearchCaseSDepartment(string text)
        {
            var item = await _dSUActiveData.GetCaseSDepartments().Result.FirstOrDefaultAsync(c => c.DeptName.ToLower() == text.ToLower());
            if (item != null)
                return item.DepartmentId;
            return null;
        }

        public async Task<LevelEdu> SearchLevelEdu(string text)
        {
            var levelEdu = await _levelEduRepository.Get().FirstOrDefaultAsync(c => c.Name.ToLower() == text.ToLower());
            if (levelEdu == null)
            {
                levelEdu = new LevelEdu(text);
                await _levelEduRepository.Create(levelEdu);
            }

            return levelEdu;
        }

        public async Task<StatusDiscipline> SearchStatusDiscipline(string text)
        {
            var statusDiscipline = await _statusDisciplineRepository.Get().FirstOrDefaultAsync(c => c.Name.ToLower() == text.ToLower());
            if (statusDiscipline == null)
            {
                statusDiscipline = new StatusDiscipline(text);
                await _statusDisciplineRepository.Create(statusDiscipline);
            }

            return statusDiscipline;
        }

        public async Task<FileType> SearchFileType(string text)
        {
            var fileType = await _fileTypeRepository.Get().FirstOrDefaultAsync(c => c.Name.ToLower() == text.ToLower());
            if (fileType == null)
            {
                fileType = new FileType(text);
                await _fileTypeRepository.Create(fileType);
            }

            return fileType;
        }
    }
}
