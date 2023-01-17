using EorDSU.Common;
using EorDSU.Models;
using EorDSU.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Repository
{
    public class FileModelRepository : GenericRepository<FileModel>, IFileModelRepository
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IConfiguration Configuration;
        public FileModelRepository(DbContext dbContext, IWebHostEnvironment appEnvironment, IConfiguration configuration) : base(dbContext)
        {
            _appEnvironment = appEnvironment;
            Configuration = configuration;
        }

        public async Task<FileModel?> CreateFileModel(IFormFile uploadedFile, int fileTypeId, int profileId)
        {
            if (Get().Any(x => x.Name == uploadedFile.FileName))
                return null;

            string path = Configuration["FileFolder"] + "/" + uploadedFile.FileName;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                await uploadedFile.CopyToAsync(fileStream);

            FileModel file = new() { Name = uploadedFile.FileName, ProfileId = profileId, Type = (FileType)fileTypeId, CreateDate = DateTime.Now };

            await Create(file);

            return file;
        }

        /// <summary>
        /// Изменение файла
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public async Task<FileModel?> EditFile(IFormFile uploadedFile, int profileId)
        {
            if (Get().Any(x => x.Name == uploadedFile.FileName))
                return null;

            string path = Configuration["FileFolder"] + uploadedFile.FileName;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                await uploadedFile.CopyToAsync(fileStream);

            FileModel file = new() { Name = uploadedFile.FileName, ProfileId = profileId, CreateDate = DateTime.Now };
            await Update(file);
            return file;
        }
    }
}
