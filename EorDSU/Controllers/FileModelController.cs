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
        public async Task<IActionResult> CreateFileModel(IFormFile formFile, int fileId, string fileName, int fileType, int profileId, string? ecp)
        {
            FileModel uploadFile = new()
            {
                Id = fileId,
                Name = formFile.FileName,
                OutputFileName = fileName,
                FileTypeId = fileType,
                ProfileId = profileId,
                CodeECP = ecp,                
            };
            if (!_fileModelRepository.Get().Any(x => x.OutputFileName == fileName || x.Name == formFile.Name))
            {
                await _addFilesOnServer.CreateFile(formFile);

                await _fileModelRepository.Create(uploadFile);
            }
            return Ok();
        }

        /// <summary>
        /// Изменение файла
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <returns></returns>
        [Route("EditFileModel")]
        [HttpPut]
        public async Task<IActionResult> EditFile(int fileId, int fileType, int profileId, string fileName, string? ecp, IFormFile? formFile)
        {
            FileModel file = _fileModelRepository.FindById(fileId);
            if (file == null)
                return BadRequest("Файл не найден");

            file.OutputFileName = fileName;
            file.UpdateDate = DateTime.Now;
            file.CodeECP = ecp ?? file.CodeECP;
            if (formFile != null)
            {
                if (!_fileModelRepository.Get().Any(x => x.Name == formFile.FileName))
                {
                    file.Name = formFile.FileName;
                    file.FileTypeId = fileType;
                    file.ProfileId = profileId;
                    await _addFilesOnServer.CreateFile(formFile);
                }
                return BadRequest("Файл с таким названием уже существует");
            }

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
