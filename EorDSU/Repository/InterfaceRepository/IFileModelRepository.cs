using EorDSU.Common.Interfaces;
using EorDSU.Models;

namespace EorDSU.Repository.InterfaceRepository
{
    public interface IFileModelRepository : IGenericRepository<FileModel>
    {
        public Task<List<FileModel>?> CreateFileModel(List<IFormFile> upload, string fileName, int fileTypeId, int profileId, string? ecp);
        public Task<FileModel?> EditFile(int fileId, string fileName, int profileId, IFormFile? upload);
    }
}
