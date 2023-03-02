using EorDSU.Common;
using EorDSU.Common.Interfaces;
using EorDSU.Models;
using EorDSU.Repository.InterfaceRepository;
using EorDSU.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Repository
{
    public class ProfileRepository : GenericRepository<Profile>, IProfileRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration Configuration;
        private readonly IWebHostEnvironment _appEnvironment;
        public ProfileRepository(DbContext dbContext, IUnitOfWork unitOfWork, IConfiguration configuration, IWebHostEnvironment appEnvironment) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
            Configuration = configuration;
            _appEnvironment = appEnvironment;
        }

        public async Task<List<DataForTableResponse>> GetData()
        {
            List<DataForTableResponse> dataForTableResponse = new();
            foreach (var item in await GetWithInclude(x => x.LevelEdu, x => x.FileModels).ToListAsync())
            {
                FillingData(dataForTableResponse, item);
            }
            return dataForTableResponse;
        }

        public async Task<List<DataForTableResponse>> GetData(int kafedraId)
        {
            List<DataForTableResponse> dataForTableResponse = new();

            foreach (var item in await GetWithInclude(x => x.LevelEdu, x => x.FileModels).Where(x => x.PersDepartmentId == kafedraId).ToListAsync())
            {
                FillingData(dataForTableResponse, item);
            }
            return dataForTableResponse;
        }

        public async Task<List<DataForTableResponse>> GetDataByFacultyId(int facultyId)
        {
            List<DataForTableResponse> dataForTableResponse = new();

            var persDepartments = await _unitOfWork.BasePersonActiveData.GetPersDepartments().Where(x => x.DivId == facultyId).ToListAsync();

            foreach (var persDepartment in persDepartments)
            {
                foreach (var item in await GetWithInclude(x => x.LevelEdu, x => x.FileModels).Where(x => x.PersDepartmentId == persDepartment.DepId).ToListAsync())
                {
                    FillingData(dataForTableResponse, item);
                }
            }
            return dataForTableResponse;
        }

        private void FillingData(List<DataForTableResponse> dataForTableResponse, Profile item)
        {
            dataForTableResponse.Add(new()
            {
                Profile = item,
                CaseCEdukind = item.CaseCEdukindId == null ? null : _unitOfWork.DSUActiveData.GetCaseCEdukindById((int)item.CaseCEdukindId),
                CaseSDepartment = item.CaseSDepartmentId == null ? null : _unitOfWork.DSUActiveData.GetCaseSDepartmentById((int)item.CaseSDepartmentId),
                Disciplines = _unitOfWork.DisciplineRepository.GetDisciplinesByProfileId(item.Id).Disciplines?.Where(x => x.Code?.Contains("Б2") == true).ToList()
            });
        }

        public Profile GetProfileById(int id)
        {
            return GetWithIncludeById(x => x.Id == id, x => x.Disciplines, x => x.FileModels, x => x.LevelEdu);
        }

        public async Task<DataResponseForSvedenOOPDGU> ParsingProfileByFile(IFormFile uploadedFile)
        {
            string path = Configuration["FileFolder"] + "/" + uploadedFile.FileName;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                await uploadedFile.CopyToAsync(fileStream);

            DataResponseForSvedenOOPDGU profile = new()
            {
                Profile = await _unitOfWork.ExcelParsingService.ParsingService(path)
            };
            profile.CaseSDepartment = profile.Profile.CaseSDepartmentId == null ? null : _unitOfWork.DSUActiveData.GetCaseSDepartmentById((int)profile.Profile.CaseSDepartmentId);
            profile.CaseCEdukind = profile.Profile.CaseCEdukindId == null ? null : _unitOfWork.DSUActiveData.GetCaseCEdukindById((int)profile.Profile.CaseCEdukindId);
            return profile;
        }

        public async Task<Profile> RemoveProfile(int id)
        {
            var profile = GetWithIncludeById(x => x.Id == id, x => x.Disciplines, x => x.FileModels, x => x.LevelEdu);
            profile.Disciplines?.ForEach(x => x.StatusDiscipline = null);
            await Remove(profile);
            return profile;
        }
    }
}
