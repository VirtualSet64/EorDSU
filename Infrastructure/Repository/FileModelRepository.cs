using Ifrastructure.Common;
using DomainServices.Entities;
using Ifrastructure.Repository.InterfaceRepository;
using DomainServices.DBService;
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
