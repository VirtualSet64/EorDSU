using BasePersonDBService.Interfaces;
using DSUContextDBService.Interfaces;
using Ifrastructure.Common;
using DomainServices.Models;
using Ifrastructure.Repository.InterfaceRepository;
using Ifrastructure.Services.Interface;
using DomainServices.DtoModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DomainServices.DBService;
using Microsoft.AspNetCore.Http;

namespace IfrastructureEorDSU.Repository
{
    public class ProfileRepository : GenericRepository<Profile>, IProfileRepository
    {
        private readonly IBasePersonActiveData _basePersonActiveData;
        private readonly IDSUActiveData _dSUActiveData;
        private readonly IExcelParsingService _excelParsingService;

        public ProfileRepository(ApplicationContext dbContext, IBasePersonActiveData basePersonActiveData, IDSUActiveData dSUActiveData, IExcelParsingService excelParsingService)
            : base(dbContext)
        {
            _basePersonActiveData = basePersonActiveData;
            _dSUActiveData = dSUActiveData;
            _excelParsingService = excelParsingService;
        }

        public async Task<List<DataForTableResponse>> GetData()
        {
            List<DataForTableResponse> dataForTableResponse = new();
            foreach (var item in await GetWithInclude(x => x.LevelEdu, x => x.FileModels, x => x.Disciplines).ToListAsync())
            {
                FillingData(dataForTableResponse, item);
            }
            return dataForTableResponse;
        }

        public async Task<List<DataForTableResponse>> GetDataByKafedraId(int kafedraId)
        {
            List<DataForTableResponse> dataForTableResponse = new();

            foreach (var item in await GetWithInclude(x => x.LevelEdu, x => x.FileModels, x => x.Disciplines).Where(x => x.PersDepartmentId == kafedraId).ToListAsync())
            {
                FillingData(dataForTableResponse, item);
            }
            return dataForTableResponse;
        }

        public async Task<List<DataForTableResponse>> GetDataByFacultyId(int facultyId)
        {
            List<DataForTableResponse> dataForTableResponse = new();

            var persDepartments = await _basePersonActiveData.GetPersDepartments().Where(x => x.DivId == facultyId).ToListAsync();

            foreach (var persDepartment in persDepartments)
            {
                foreach (var item in await GetWithInclude(x => x.LevelEdu, x => x.FileModels, x => x.Disciplines).Where(x => x.PersDepartmentId == persDepartment.DepId).ToListAsync())
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
                CaseCEdukind = item.CaseCEdukindId == null ? null : _dSUActiveData.GetCaseCEdukindById((int)item.CaseCEdukindId),
                CaseSDepartment = item.CaseSDepartmentId == null ? null : _dSUActiveData.GetCaseSDepartmentById((int)item.CaseSDepartmentId),
                Disciplines = item.Disciplines.Where(x => x.Code?.Contains("Б2") == true).ToList()
            });
        }

        public async Task<List<Profile>> GetProfileByFacultyId(int facultyId)
        {
            var departments = _dSUActiveData.GetCaseSDepartmentByFacultyId(facultyId);
            List<Profile> profiles = new();
            foreach (var item in departments)
            {
                profiles.AddRange(await Get().Where(x => x.CaseSDepartmentId == item.DepartmentId).ToListAsync());
            }
            return profiles;
        }

        public Profile GetProfileById(int id)
        {
            return GetWithIncludeById(x => x.Id == id, x => x.Disciplines, x => x.FileModels, x => x.LevelEdu);
        }

        public async Task<DataResponseForSvedenOOPDGU> ParsingProfileByFile(string path)
        {
            DataResponseForSvedenOOPDGU profile = new()
            {
                Profile = await _excelParsingService.ParsingService(path)
            };
            profile.CaseSDepartment = profile.Profile.CaseSDepartmentId == null ? null : _dSUActiveData.GetCaseSDepartmentById((int)profile.Profile.CaseSDepartmentId);
            profile.CaseCEdukind = profile.Profile.CaseCEdukindId == null ? null : _dSUActiveData.GetCaseCEdukindById((int)profile.Profile.CaseCEdukindId);
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
