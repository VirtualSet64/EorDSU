using EorDSU.Common.Interfaces;
using EorDSU.Models;

namespace EorDSU.Repository.InterfaceRepository
{
    public interface IFileModelRepository : IGenericRepository<FileModel>
    {
        public Task<FileModel?> CreateFileModel(IFormFile file, int fileTypeId, int profileId);
        public Task<FileModel?> EditFile(IFormFile uploadedFile, int profileId);
    }
}
