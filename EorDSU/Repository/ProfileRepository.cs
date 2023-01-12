using BasePersonDBService.Interfaces;
using DSUContextDBService.Interfaces;
using EorDSU.Common;
using EorDSU.Common.Interfaces;
using EorDSU.Models;
using EorDSU.Repository.InterfaceRepository;
using EorDSU.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sentry;

namespace EorDSU.Repository
{
    public class ProfileRepository : GenericRepository<Profile>, IProfileRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration Configuration;
        public ProfileRepository(DbContext dbContext, IUnitOfWork unitOfWork, IConfiguration configuration) : base(dbContext)
        {
            _unitOfWork = unitOfWork;
            Configuration = configuration;
        }

        public async Task<List<DataResponseForSvedenOOPDGU>> GetData()
        {
            List<DataResponseForSvedenOOPDGU> dataResponseForSvedenOOPDGUs = new();
            foreach (var item in await Get().ToListAsync())
            {
                FillingData(dataResponseForSvedenOOPDGUs, item);
            }
            return dataResponseForSvedenOOPDGUs;
        }

        public async Task<List<DataResponseForSvedenOOPDGU>> GetData(int cafedraId)
        {
            List<DataResponseForSvedenOOPDGU> dataResponseForSvedenOOPDGUs = new();

            foreach (var item in await Get().Where(x => x.PersDepartmentId == cafedraId).ToListAsync())
            {
                FillingData(dataResponseForSvedenOOPDGUs, item);
            }
            return dataResponseForSvedenOOPDGUs;
        }

        public async Task<List<DataResponseForSvedenOOPDGU>> GetDataFacultyById(int facultyId)
        {
            List<DataResponseForSvedenOOPDGU> dataResponseForSvedenOOPDGUs = new();

            var persDepartments = await _unitOfWork.BasePersonActiveData.GetPersDepartments().Where(x => x.DivId == facultyId).ToListAsync();

            foreach (var persDepartment in persDepartments)
            {
                foreach (var item in await Get().Where(x => x.PersDepartmentId == persDepartment.DepId).ToListAsync())
                {
                    FillingData(dataResponseForSvedenOOPDGUs, item);
                }
            }
            return dataResponseForSvedenOOPDGUs;
        }

        private void FillingData(List<DataResponseForSvedenOOPDGU> dataResponseForSvedenOOPDGUs, Profile item)
        {
            dataResponseForSvedenOOPDGUs.Add(new()
            {
                Profiles = item,
                CaseCEdukind = _unitOfWork.DSUActiveData.GetCaseCEdukindById((int)item.CaseCEdukindId),
                CaseSDepartment = _unitOfWork.DSUActiveData.GetCaseSDepartmentById((int)item.CaseSDepartmentId),
                SrokDeystvGosAccred = Configuration["SrokDeystvGosAccred"],
            });
        }

        public Profile GetProfileById(int id)
        {
            return GetWithIncludeById(x => x.Id == id, x => x.Disciplines, x => x.FileModels, x => x.LevelEdu);
        }

        public async Task<Profile> RemoveProfile(int id)
        {
            var profile = GetWithIncludeById(x => x.Id == id, x => x.Disciplines, x => x.FileModels, c => c.LevelEdu);
            await Remove(profile);
            return profile;
        }
    }
}
