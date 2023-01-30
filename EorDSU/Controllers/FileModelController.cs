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
        public async Task<IActionResult> CreateFileModel(IFormFileCollection uploads, List<string> fileNameList, int fileTypeId, int profileId)
        {
            List<FileModel> files = await _unitOfWork.FileModelRepository.CreateFileModel(uploads, fileNameList, fileTypeId, profileId);
            if (files == null)
                return BadRequest();
            return Ok(files);
        }

        /// <summary>
        /// Изменение файла
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        [Route("EditFileModel")]
        [HttpPut]
        public async Task<IActionResult> EditFile(IFormFileCollection uploads, List<string> fileNameList, int profileId)
        {
            if (profileId <= 0)
                return BadRequest();

            List<FileModel> files = await _unitOfWork.FileModelRepository.EditFile(uploads, fileNameList, profileId);
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
