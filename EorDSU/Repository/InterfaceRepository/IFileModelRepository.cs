using EorDSU.Common.Interfaces;
using EorDSU.Models;

namespace EorDSU.Repository.InterfaceRepository
{
    public interface IFileModelRepository : IGenericRepository<FileModel>
    {
        public Task<FileModel?> CreateFileModel(IFormFile upload, string fileName, int fileTypeId, int profileId);
        public Task<FileModel?> EditFile(int fileId, IFormFile? upload, string fileName, int profileId);
    }
}
