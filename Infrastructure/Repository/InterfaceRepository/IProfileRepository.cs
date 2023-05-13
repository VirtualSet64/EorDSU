using Infrastructure.Common.Interfaces;
using DomainServices.Entities;
using DomainServices.DtoModels;

namespace Infrastructure.Repository.InterfaceRepository
{
    public interface IProfileRepository : IGenericRepository<Profile>
    {
        public List<DataForTableResponse> GetDataForOopDgu();
        public List<DataForTableResponse> GetDataOpop2();
        public List<DataForTableResponse> GetDataByKafedraId(int cafedraId);
        public List<DataForTableResponse> GetDataByFacultyId(int facultyId);
        public Task<List<Profile>> GetProfileByFacultyId(int facultyId);
        public Profile GetProfileById(int id);
        public Task<DataResponseForSvedenOOPDGU> ParsingProfileByFile(string path);
        public Task<Profile> UpdateProfile(Profile profile);
        public Task<Profile> RemoveProfile(int profileId);
    }
}
