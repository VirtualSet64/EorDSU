using EorDSU.Common.Interfaces;
using EorDSU.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EorDSU.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FileModelController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FileModelController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Добавление файлов
        /// </summary>
        /// <param name="uploadedFile">Файл</param>
        /// <param name="fileTypeId">Тип Файла</param>
        /// <param name="profileId">Профиль</param>
        /// <returns></returns>
        [Route("CreateFileModel")]
        [HttpPost]
        public async Task<IActionResult> CreateFileModel(IFormFile upload, string fileName, int fileTypeId, int profileId)
        {
            FileModel file = await _unitOfWork.FileModelRepository.CreateFileModel(upload, fileName, fileTypeId, profileId);
            if (file == null)
                return BadRequest();
            return Ok(file);
        }

        /// <summary>
        /// Изменение файла
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        [Route("EditFileModel")]
        [HttpPut]
        public async Task<IActionResult> EditFile(IFormFile upload, string fileName, int profileId)
        {
            if (profileId <= 0)
                return BadRequest();

            FileModel files = await _unitOfWork.FileModelRepository.EditFile(upload, fileName, profileId);
            return Ok(files);
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
            await _unitOfWork.FileModelRepository.Remove(fileId);
            return Ok();
        }
    }
}
