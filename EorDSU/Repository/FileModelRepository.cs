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

        /// <summary>
        /// Создание файлов
        /// </summary>
        /// <param name="uploads"></param>
        /// <param name="fileTypeId"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public async Task<List<FileModel>?> CreateFileModel(IFormFileCollection uploads, int fileTypeId, int profileId)
        {
            List<FileModel> files = new();
            foreach (var uploadedFile in uploads)
            {
                if (Get().Any(x => x.Name == uploadedFile.FileName))
                    return null;

                string path = Configuration["FileFolder"] + "/" + uploadedFile.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    await uploadedFile.CopyToAsync(fileStream);

                FileModel file = new()
                {
                    Name = uploadedFile.FileName,
                    ProfileId = profileId,
                    Type = (FileType)fileTypeId,
                    CreateDate = DateTime.Now
                };                
                await Create(file);

                files.Add(file);
            }
            return files;
        }

        /// <summary>
        /// Изменение файлов
        /// </summary>
        /// <param name="uploads"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public async Task<List<FileModel>?> EditFile(IFormFileCollection uploads, int profileId)
        {
            List<FileModel> files = new();
            foreach (var uploadedFile in uploads)
            {
                if (Get().Any(x => x.Name == uploadedFile.FileName))
                    return null;

                string path = Configuration["FileFolder"] + uploadedFile.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    await uploadedFile.CopyToAsync(fileStream);

                FileModel file = new()
                {
                    Name = uploadedFile.FileName,
                    ProfileId = profileId,
                    UpdateDate = DateTime.Now
                };
                await Update(file);

                files.Add(file);
            }
            return files;
        }
    }
}
