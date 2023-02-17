using EorDSU.Common;
using EorDSU.Models;
using EorDSU.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Repository
{
    public class FileRPDRepository : GenericRepository<FileRPD>, IFileRPDRepository
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IConfiguration Configuration;
        public FileRPDRepository(DbContext dbContext, IWebHostEnvironment appEnvironment, IConfiguration configuration) : base(dbContext)
        {
            _appEnvironment = appEnvironment;
            Configuration = configuration;
        }

        public async Task<FileRPD?> CreateFileRPD(IFormFile uploadedFile, int disciplineId, string? ecp)
        {
            if (Get().Any(x => x.Name == uploadedFile.FileName))
                return null;

            string path = Configuration["FileFolder"] + uploadedFile.FileName;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                await uploadedFile.CopyToAsync(fileStream);
            
            var file = new FileRPD() { Name = uploadedFile.FileName, DisciplineId = disciplineId, CodeECP = ecp, CreateDate = DateTime.Now };
            await Create(file);
            return file;
        }
    }
}
