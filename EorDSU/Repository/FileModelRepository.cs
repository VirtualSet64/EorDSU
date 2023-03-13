using EorDSU.Common;
using EorDSU.DBService;
using EorDSU.Models;
using EorDSU.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Repository
{
    public class FileModelRepository : GenericRepository<FileModel>, IFileModelRepository
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IConfiguration Configuration;
        private readonly IFileTypeRepository _fileTypeRepository;
        public FileModelRepository(ApplicationContext dbContext, IWebHostEnvironment appEnvironment, IConfiguration configuration, IFileTypeRepository fileTypeRepository) : base(dbContext)
        {
            _appEnvironment = appEnvironment;
            Configuration = configuration;
            _fileTypeRepository = fileTypeRepository;
        }

        /// <summary>
        /// Создание файлов
        /// </summary>
        /// <param name="uploadFiles"></param>
        /// <param name="fileName"></param>
        /// <param name="fileTypeId"></param>
        /// <param name="profileId"></param>
        /// <param name="ecp"></param>
        /// <returns></returns>
        public async Task<List<FileModel>?> CreateFileModel(List<IFormFile> uploadFiles, string fileName, int fileTypeId, int profileId, string? ecp)
        {
            List<FileModel> files = new();
            foreach (var uploadFile in uploadFiles)
            {
                if (Get().Any(x => x.Name == uploadFile.FileName))
                    return null;

                string path = Configuration["FileFolder"] + "/" + uploadFile.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    await uploadFile.CopyToAsync(fileStream);

                FileModel file = new()
                {
                    Name = uploadFile.FileName,
                    OutputFileName = fileName,
                    ProfileId = profileId,                    
                    Type = _fileTypeRepository.FindById(fileTypeId),
                    FileTypeId = fileTypeId,
                    CodeECP = ecp
                };

                await Create(file);
                files.Add(file);
            }
            return files;
        }

        /// <summary>
        /// Изменение файлов
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="fileName"></param>
        /// <param name="profileId"></param>
        /// <param name="upload"></param>
        /// <param name="ecp"></param>
        /// <returns></returns>
        public async Task<FileModel?> EditFile(int fileId, string fileName, int profileId, IFormFile? upload, string? ecp)
        {
            FileModel file = FindById(fileId);
            file.OutputFileName = fileName;
            file.UpdateDate = DateTime.Now;
            if (ecp != null)
                file.CodeECP = ecp;
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
