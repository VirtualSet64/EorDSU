using Ifrastructure.Common;
using DomainServices.Entities;
using Ifrastructure.Repository.InterfaceRepository;
using DomainServices.DBService;
using DomainServices.DtoModels;

namespace Ifrastructure.Repository
{
    public class FileRPDRepository : GenericRepository<FileRPD>, IFileRPDRepository
    {
        public FileRPDRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Создание файла РПД
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <returns></returns>
        public async Task<FileRPD?> CreateFileRPD(UploadFileRPD uploadedFile)
        {
            var file = new FileRPD()
            {
                Name = uploadedFile.UploadedFile?.FileName,
                DisciplineId = uploadedFile.DisciplineId,
                PersonId = uploadedFile.AuthorId,
                CodeECP = uploadedFile.Ecp
            };
            await Create(file);
            return file;
        }
    }
}
