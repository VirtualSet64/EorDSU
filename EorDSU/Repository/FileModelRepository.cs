using EorDSU.Common;
using EorDSU.Common.Interfaces;
using EorDSU.Models;
using EorDSU.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Repository
{
    public class FileModelRepository : GenericRepository<FileModel>, IFileModelRepository
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IConfiguration Configuration;
        private readonly IUnitOfWork _unitOfWork;
        public FileModelRepository(DbContext dbContext, IWebHostEnvironment appEnvironment, IConfiguration configuration, IUnitOfWork unitOfWork) : base(dbContext)
        {
            _appEnvironment = appEnvironment;
            Configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Создание файлов
        /// </summary>
        /// <param name="uploads"></param>
        /// <param name="fileNameList"></param>
        /// <param name="fileTypeId"></param>
        /// <param name="profileId"></param>
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
                    Type = _unitOfWork.FileTypeRepository.FindById(fileTypeId),
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
        /// <param name="uploads"></param>
        /// <param name="fileNameList"></param>
        /// <param name="profileId"></param>
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
