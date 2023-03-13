using Ifrastructure.Common;
using DomainServices.Models;
using Ifrastructure.Repository.InterfaceRepository;
using Microsoft.Extensions.Configuration;
using DomainServices.DBService;
using Microsoft.AspNetCore.Http;
using DomainServices.DtoModels;

namespace Ifrastructure.Repository
{
    public class FileRPDRepository : GenericRepository<FileRPD>, IFileRPDRepository
    {
        public FileRPDRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }

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
