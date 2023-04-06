﻿using BasePersonDBService.Interfaces;
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

        public async Task<List<DataForTableResponse>> GetData()
        {
            List<DataForTableResponse> dataForTableResponse = new();
            foreach (var item in GetWithInclude(x => x.LevelEdu, x => x.FileModels))
            {
                FillingData(ref dataForTableResponse, item);
            }
            return dataForTableResponse;
        }

        public async Task<List<DataForTableResponse>> GetDataByKafedraId(int kafedraId)
        {
            List<DataForTableResponse> dataForTableResponse = new();

            foreach (var item in await GetWithInclude(x => x.LevelEdu, x => x.FileModels).Where(x => x.ListPersDepartmentsId.Any(c => c.PersDepartmentId == kafedraId)).ToListAsync())
            {
                FillingData(ref dataForTableResponse, item);
            }
            return dataForTableResponse;
        }

        public async Task<List<DataForTableResponse>> GetDataByFacultyId(int facultyId)
        {
            List<DataForTableResponse> dataForTableResponse = new();

            var caseSDepartments = await _dSUActiveData.GetCaseSDepartmentByFacultyId(facultyId).ToListAsync();

            foreach (var caseSDepartment in caseSDepartments)
            {
                foreach (var item in await GetWithInclude(x => x.LevelEdu, x => x.FileModels).Where(x => x.CaseSDepartmentId == caseSDepartment.DepartmentId).ToListAsync())
                {
                    FillingData(ref dataForTableResponse, item);
                }
            }
            return dataForTableResponse;
        }

        private void FillingData(ref List<DataForTableResponse> dataForTableResponse, Profile item)
        {
            dataForTableResponse.Add(new()
            {
                Profile = item,
                CaseCEdukind = item.CaseCEdukindId == null ? null : _dSUActiveData.GetCaseCEdukindById((int)item.CaseCEdukindId),
                CaseSDepartment = item.CaseSDepartmentId == null ? null : _dSUActiveData.GetCaseSDepartmentById((int)item.CaseSDepartmentId),
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
