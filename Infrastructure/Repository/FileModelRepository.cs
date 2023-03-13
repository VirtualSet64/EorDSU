using Ifrastructure.Common;
using DomainServices.Models;
using Ifrastructure.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;
using DomainServices.DBService;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using DomainServices.DtoModels;

namespace Ifrastructure.Repository
{
    public class FileModelRepository : GenericRepository<FileModel>, IFileModelRepository
    {
        private readonly IFileTypeRepository _fileTypeRepository;
        public FileModelRepository(ApplicationContext dbContext, IFileTypeRepository fileTypeRepository) : base(dbContext)
        {
            _fileTypeRepository = fileTypeRepository;
        }

        /// <summary>
        /// Создание файлов
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <param name="fileName"></param>
        /// <param name="fileTypeId"></param>
        /// <param name="profileId"></param>
        /// <param name="ecp"></param>
        /// <returns></returns>
        public async Task<FileModel?> CreateFileModel(UploadFileForFileModel uploadFile)
        {
            FileModel file = new()
            {
                Name = uploadFile.FileName,
                OutputFileName = uploadFile.FileName,
                ProfileId = uploadFile.ProfileId,
                Type = _fileTypeRepository.FindById(uploadFile.FileType),
                FileTypeId = uploadFile.FileType,
                CodeECP = uploadFile.Ecp
            };

            await Create(file);
            return file;
        }
    }
}
