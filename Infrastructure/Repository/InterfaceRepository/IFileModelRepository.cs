using Ifrastructure.Common.Interfaces;
using DomainServices.Models;
using Microsoft.AspNetCore.Http;
using DomainServices.DtoModels;

namespace Ifrastructure.Repository.InterfaceRepository
{
    public interface IFileModelRepository : IGenericRepository<FileModel>
    {
        public Task<FileModel?> CreateFileModel(UploadFileForFileModel uploadFile);
    }
}
