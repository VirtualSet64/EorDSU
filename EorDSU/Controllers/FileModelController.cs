using DomainServices.DtoModels;
using DomainServices.Entities;
using EorDSU.Services.Interfaces;
using Ifrastructure.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EorDSU.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FileModelController : Controller
    {
        private readonly IFileModelRepository _fileModelRepository;
        private readonly IAddFileOnServer _addFilesOnServer;

        public FileModelController(IFileModelRepository fileModelRepository, IAddFileOnServer addFilesOnServer)
        {
            _fileModelRepository = fileModelRepository;
            _addFilesOnServer = addFilesOnServer;
        }

        /// <summary>
        /// Добавление файлов
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <returns></returns>
        [Route("CreateFileModel")]
        [HttpPost]
        public async Task<IActionResult> CreateFileModel(List<UploadFileForFileModel> uploadedFile)
        {
            List<FileModel> files = new();
            foreach (var uploadFile in uploadedFile)
            {
                if (!_fileModelRepository.Get().Any(x => x.Name == uploadFile.FileName))
                {
                    if (uploadFile.UploadedFile != null)
                        await _addFilesOnServer.CreateFile(uploadFile.UploadedFile);

                    files.Add(await _fileModelRepository.CreateFileModel(uploadFile));
                }
            }

            if (files == null || files.Count == 0)
                return BadRequest("Файл с таким названием уже существует");
            return Ok();
        }

        /// <summary>
        /// Изменение файла
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <returns></returns>
        [Route("EditFileModel")]
        [HttpPut]
        public async Task<IActionResult> EditFile(UploadFileForFileModel uploadFile)
        {
            FileModel file = _fileModelRepository.FindById((int)uploadFile.FileId);
            if (file == null)
                return BadRequest("Ошибка изменения файла");

            file.OutputFileName = uploadFile.FileName;
            file.UpdateDate = DateTime.Now;
            if (uploadFile.UploadedFile != null)
            {
                if (!_fileModelRepository.Get().Any(x => x.Name == uploadFile.UploadedFile.FileName))
                {
                    file.Name = uploadFile.UploadedFile.FileName;
                    file.ProfileId = uploadFile.ProfileId;
                    await _addFilesOnServer.CreateFile(uploadFile.UploadedFile);                    
                }
            }
            if (uploadFile.Ecp != null)
                file.CodeECP = uploadFile.Ecp;

            await _fileModelRepository.Update(file);
            return Ok();
        }

        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [Route("DeleteFileModel")]
        [HttpDelete]
        public async Task<IActionResult> DeleteFile(int fileId)
        {
            await _fileModelRepository.Remove(fileId);
            return Ok();
        }
    }
}
