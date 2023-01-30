using EorDSU.Common;
using EorDSU.Common.Interfaces;
using EorDSU.Models;
using EorDSU.Repository.InterfaceRepository;
using EorDSU.ResponseModel;
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
                foreach (var item in Get().Where(x => x.PersDepartmentId == persDepartment.DepId).ToList())
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
                Profile = item,
                CaseCEdukind = _unitOfWork.DSUActiveData.GetCaseCEdukindById((int)item.CaseCEdukindId),
                CaseSDepartment = _unitOfWork.DSUActiveData.GetCaseSDepartmentById((int)item.CaseSDepartmentId)
            });
        }

        public Profile GetProfileById(int id)
        {
            return GetWithIncludeById(x => x.Id == id, x => x.Disciplines, x => x.FileModels, x => x.LevelEdu);
        }

        public async Task<DataResponseForSvedenOOPDGU> ParsedProfileForPreview(IFormFile uploadedFile)
        {
            string path = Configuration["FileFolder"] + "/" + uploadedFile.FileName;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                await uploadedFile.CopyToAsync(fileStream);

            DataResponseForSvedenOOPDGU profile = new()
            {
                Profile = await _unitOfWork.ExcelParsingService.ParsingService(path)                
            };
            profile.CaseSDepartment = _unitOfWork.DSUActiveData.GetCaseSDepartmentById((int)profile.Profile.CaseSDepartmentId);
            profile.CaseCEdukind = _unitOfWork.DSUActiveData.GetCaseCEdukindById((int)profile.Profile.CaseCEdukindId);
            return profile;
        }

        public async Task<Profile> RemoveProfile(int id)
        {
            var profile = GetWithIncludeById(x => x.Id == id, x => x.Disciplines, x => x.FileModels, c => c.LevelEdu);
            await Remove(profile);
            return profile;
        }
    }
}
