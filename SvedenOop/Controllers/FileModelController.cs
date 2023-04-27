using DomainServices.Entities;
using SvedenOop.Services.Interfaces;
using Ifrastructure.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SvedenOop.Controllers
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
        /// <param name="fileName"></param>
        /// <param name="fileType"></param>
        /// <param name="profileId"></param>
        /// <param name="linkToFile"></param>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [Route("CreateFileModel")]
        [HttpPost]
        public async Task<IActionResult> CreateFileModel(string fileName, int fileType, int profileId, string? linkToFile, IFormFile? formFile)
        {
            FileModel file = new()
            {
                OutputFileName = fileName,
                FileTypeId = fileType,
                ProfileId = profileId,
            };
            if (linkToFile != null)
            {
                file.Name = linkToFile.Split("/")[^1];
                file.LinkToFile = linkToFile;
            }
            else
                file.Name = formFile?.FileName;
            
            if (!_fileModelRepository.Get().Any(x => x.Name == file.Name))
            {
                if (formFile != null)
                    await _addFilesOnServer.CreateFile(formFile);
                else
                    await _fileModelRepository.Create(file);
                return Ok();
            }
            return BadRequest("Файл с таким названием уже существует");
        }

        /// <summary>
        /// Изменение файла
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="fileName"></param>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [Route("EditFileModel")]
        [HttpPut]
        public async Task<IActionResult> EditFile(int fileId, string fileName, IFormFile? formFile)
        {
            FileModel file = _fileModelRepository.FindById(fileId);
            if (file == null)
                return BadRequest("Файл не найден");

            file.OutputFileName = fileName;
            file.UpdateDate = DateTime.Now;

            if (formFile != null)
            {
                if (!_fileModelRepository.Get().Any(x => x.Name == formFile.FileName))
                {
                    file.Name = formFile.FileName;
                    file.CodeECP = Guid.NewGuid().ToString().ToUpper();
                    await _addFilesOnServer.CreateFile(formFile);
                }
                else
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
