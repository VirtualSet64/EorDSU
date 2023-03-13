using EorDSU.Repository.InterfaceRepository;
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

        public FileModelController(IFileModelRepository fileModelRepository)
        {
            _fileModelRepository = fileModelRepository;
        }

        /// <summary>
        /// Добавление файлов
        /// </summary>
        /// <param name="uploadedFile">Файл</param>
        /// <param name="fileName">Файл</param>
        /// <param name="fileType">Тип Файла</param>
        /// <param name="profileId">Профиль</param>
        /// <param name="ecp">Код ЭЦП</param>
        /// <returns></returns>
        [Route("CreateFileModel")]
        [HttpPost]
        public async Task<IActionResult> CreateFileModel(List<IFormFile> uploadedFile, string fileName, int fileType, int profileId, string? ecp)
        {
            var files = await _fileModelRepository.CreateFileModel(uploadedFile, fileName, fileType, profileId, ecp);
            if (files == null)
                return BadRequest("Файл с таким названием уже существует");
            return Ok();
        }

        /// <summary>
        /// Изменение файла
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="uploadedFile"></param>
        /// <param name="fileName"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        [Route("EditFileModel")]
        [HttpPut]
        public async Task<IActionResult> EditFile(int fileId, string fileName, int profileId, IFormFile? uploadedFile, string? ecp)
        {
            var files = await _fileModelRepository.EditFile(fileId, fileName, profileId, uploadedFile, ecp);
            if (files == null)
                return BadRequest("Ошибка изменения файла");
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
