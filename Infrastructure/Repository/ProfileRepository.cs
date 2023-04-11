using DSUContextDBService.Interfaces;
using Ifrastructure.Common;
using DomainServices.Entities;
using Ifrastructure.Repository.InterfaceRepository;
using Ifrastructure.Services.Interface;
using DomainServices.DtoModels;
using Microsoft.EntityFrameworkCore;
using DomainServices.DBService;
using Infrastructure.Repository.InterfaceRepository;

namespace IfrastructureSvedenOop.Repository
{
    public class ProfileRepository : GenericRepository<Profile>, IProfileRepository
    {
        private readonly IDSUActiveData _dSUActiveData;
        private readonly IExcelParsingService _excelParsingService;
        private readonly IProfileKafedrasRepository _profileKafedrasRepository;

        public ProfileRepository(ApplicationContext dbContext, IDSUActiveData dSUActiveData, IExcelParsingService excelParsingService, IProfileKafedrasRepository profileKafedrasRepository)
            : base(dbContext)
        {
            _dSUActiveData = dSUActiveData;
            _excelParsingService = excelParsingService;
            _profileKafedrasRepository = profileKafedrasRepository;
        }

        public List<DataForTableResponse> GetDataForOopDgu()
        {
            List<DataForTableResponse> dataForTableResponse = new();
            var profiles = GetWithInclude(x => x.LevelEdu, x => x.FileModels, x => x.ListPersDepartmentsId);

            FillingData(ref dataForTableResponse, ref profiles);
            return dataForTableResponse;
        }

        public List<DataForTableResponse> GetDataOpop2()
        {
            List<DataForTableResponse> dataForTableResponse = new();
            var profiles = GetWithInclude(x => x.LevelEdu, x => x.FileModels, x => x.ListPersDepartmentsId, x => x.Disciplines);

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
            return GetWithIncludeById(x => x.Id == id, x => x.LevelEdu, x => x.Disciplines, x => x.FileModels, x => x.ListPersDepartmentsId);
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

        public async Task<Profile> UpdateProfile(Profile profile)
        {
            profile.UpdateDate = DateTime.Now;

            var profileKafedras = _profileKafedrasRepository.Get();
            if (profile.ListPersDepartmentsId != null)
            {
                await _profileKafedrasRepository.RemoveRange(profileKafedras.Where(x => x.ProfileId == profile.Id));
                foreach (var item in profile.ListPersDepartmentsId)
                {
                    
                    if (!profileKafedras.Any(x => x.PersDepartmentId == item.PersDepartmentId && x.ProfileId == item.ProfileId))
                        await _profileKafedrasRepository.Create(item);
                }
            }
            await Update(profile);
            return profile;
        }

        public async Task<Profile> RemoveProfile(int id)
        {
            var profiles = GetWithInclude(x => x.FileModels, x => x.LevelEdu, x => x.ListPersDepartmentsId).Include(x => x.Disciplines).ThenInclude(c => c.FileRPD);
            var profile = profiles.FirstOrDefault(x => x.Id == id);
            await Remove(profile);
            return profile;
        }
    }
}
