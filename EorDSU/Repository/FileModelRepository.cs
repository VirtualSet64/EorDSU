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
        /// <param name="fileNameList"></param>
        /// <param name="fileTypeId"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public async Task<FileModel?> CreateFileModel(IFormFile upload, string fileName, int fileTypeId, int profileId)
        {
            if (Get().Any(x => x.Name == upload.FileName))
                return null;

            string path = Configuration["FileFolder"] + "/" + upload.FileName;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                await upload.CopyToAsync(fileStream);

            FileModel file = new()
            {
                Name = upload.FileName,
                OutputFileName = fileName,
                ProfileId = profileId,
                Type = (FileType)fileTypeId,
                CreateDate = DateTime.Now
            };
            await Create(file);
            return file;
        }

        /// <summary>
        /// Изменение файлов
        /// </summary>
        /// <param name="uploads"></param>
        /// <param name="fileNameList"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public async Task<FileModel?> EditFile(int fileId, IFormFile? upload, string fileName, int profileId)
        {
            FileModel file = await FindById(fileId);
            file.OutputFileName = fileName;
            file.UpdateDate = DateTime.Now;
            if (upload != null)
            {
                if (Get().Any(x => x.Name == upload.FileName))
                    return null;

                string path = Configuration["FileFolder"] + upload.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    await upload.CopyToAsync(fileStream);

                file.Name = upload.FileName;
                file.ProfileId = profileId;
            }
            await Update(file);
            return file;
        }
    }
}
