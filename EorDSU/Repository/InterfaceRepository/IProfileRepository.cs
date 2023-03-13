using EorDSU.Common.Interfaces;
using EorDSU.Models;
using EorDSU.ViewModels;

namespace EorDSU.Repository.InterfaceRepository
{
    public interface IProfileRepository : IGenericRepository<Profile>
    {
        public Task<List<DataForTableResponse>> GetData();
        public Task<List<DataForTableResponse>> GetDataByKafedraId(int cafedraId);
        public Task<List<DataForTableResponse>> GetDataByFacultyId(int facultyId);
        public Task<List<Profile>> GetProfileByFacultyId(int facultyId);
        public Profile GetProfileById(int id);
        public Task<DataResponseForSvedenOOPDGU> ParsingProfileByFile(IFormFile uploadedFile);      
        public Task<Profile> RemoveProfile(int profileId);
    }
}
