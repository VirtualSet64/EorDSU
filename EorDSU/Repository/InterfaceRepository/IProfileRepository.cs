using EorDSU.Common.Interfaces;
using EorDSU.Models;
using EorDSU.ResponseModel;

namespace EorDSU.Repository.InterfaceRepository
{
    public interface IProfileRepository : IGenericRepository<Profile>
    {
        public Task<List<DataResponseForSvedenOOPDGU>> GetData();
        public Task<List<DataResponseForSvedenOOPDGU>> GetData(int cafedraId);
        public Task<List<DataResponseForSvedenOOPDGU>> GetDataFacultyById(int facultyId);
        public Profile GetProfileById(int id);
        public Task<DataResponseForSvedenOOPDGU> ParsedProfileForPreview(IFormFile uploadedFile);
        public Task<Profile> RemoveProfile(int profileId);
    }
}
