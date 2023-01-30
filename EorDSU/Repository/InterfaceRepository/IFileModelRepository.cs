using EorDSU.Common.Interfaces;
using EorDSU.Models;

namespace EorDSU.Repository.InterfaceRepository
{
    public interface IFileModelRepository : IGenericRepository<FileModel>
    {
        public Task<List<FileModel>?> CreateFileModel(IFormFileCollection uploads, List<string> fileNameList, int fileTypeId, int profileId);
        public Task<List<FileModel>?> EditFile(IFormFileCollection uploads, List<string> fileNameList, int profileId);
    }
}
