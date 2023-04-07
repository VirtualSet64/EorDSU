using DSUContextDBService.Interfaces;
using Ifrastructure.Common;
using DomainServices.Entities;
using Ifrastructure.Repository.InterfaceRepository;
using Ifrastructure.Services.Interface;
using DomainServices.DtoModels;
using Microsoft.EntityFrameworkCore;
using DomainServices.DBService;

namespace IfrastructureSvedenOop.Repository
{
    public class ProfileRepository : GenericRepository<Profile>, IProfileRepository
    {
        private readonly IDSUActiveData _dSUActiveData;
        private readonly IExcelParsingService _excelParsingService;

        public ProfileRepository(ApplicationContext dbContext, IDSUActiveData dSUActiveData, IExcelParsingService excelParsingService)
            : base(dbContext)
        {
            _dSUActiveData = dSUActiveData;
            _excelParsingService = excelParsingService;
        }

        public List<DataForTableResponse> GetData()
        {
            List<DataForTableResponse> dataForTableResponse = new();
            var profiles = GetWithInclude(x => x.LevelEdu, x => x.FileModels, x => x.ListPersDepartmentsId);

            FillingData(ref dataForTableResponse, ref profiles);
            return dataForTableResponse;
        }

        public List<DataForTableResponse> GetDataByKafedraId(int kafedraId)
        {
            List<DataForTableResponse> dataForTableResponse = new();
            var profiles = GetWithInclude(x => x.LevelEdu, x => x.FileModels, x => x.ListPersDepartmentsId).Where(x => x.ListPersDepartmentsId.Any(c => c.PersDepartmentId == kafedraId));

            FillingData(ref dataForTableResponse, ref profiles);
            return dataForTableResponse;
        }

        public List<DataForTableResponse> GetDataByFacultyId(int facultyId)
        {
            List<DataForTableResponse> dataForTableResponse = new();
            var caseSDepartments = _dSUActiveData.GetCaseSDepartmentByFacultyId(facultyId);

            foreach (var caseSDepartment in caseSDepartments)
            {
                var profiles = GetWithInclude(x => x.LevelEdu, x => x.FileModels, x => x.ListPersDepartmentsId).Where(x => x.CaseSDepartmentId == caseSDepartment.DepartmentId);

                FillingData(ref dataForTableResponse, ref profiles);
            }
            return dataForTableResponse;
        }

        private void FillingData(ref List<DataForTableResponse> dataForTableResponse, ref IQueryable<Profile> profiles)
        {
            var departments = _dSUActiveData.GetCaseSDepartments();
            var edukinds = _dSUActiveData.GetCaseCEdukinds();
            foreach (var item in profiles)
            {
                dataForTableResponse.Add(new()
                {
                    Profile = item,
                    CaseCEdukind = item.CaseCEdukindId == null ? null : edukinds.FirstOrDefault(x => x.EdukindId == item.CaseCEdukindId),
                    CaseSDepartment = item.CaseSDepartmentId == null ? null : departments.FirstOrDefault(x => x.DepartmentId == item.CaseSDepartmentId),
                });
            }
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
            return GetWithIncludeById(x => x.Id == id, x => x.LevelEdu, x => x.Disciplines, x => x.FileModels);
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
