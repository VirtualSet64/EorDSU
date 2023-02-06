using EorDSU.Common.Interfaces;
using EorDSU.Models;
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
        private readonly IUnitOfWork _unitOfWork;

        public FileModelController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Добавление файлов
        /// </summary>
        /// <param name="uploadedFile">Файл</param>
        /// <param name="fileName">Файл</param>
        /// <param name="fileType">Тип Файла</param>
        /// <param name="profileId">Профиль</param>
        /// <returns></returns>
        [Route("CreateFileModel")]
        [HttpPost]
        public async Task<IActionResult> CreateFileModel(IFormFile uploadedFile, string fileName, int fileType, int profileId)
        {
            FileModel file = await _unitOfWork.FileModelRepository.CreateFileModel(uploadedFile, fileName, fileType, profileId);
            if (file == null)
                return BadRequest();
            return Ok(file);
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
        public async Task<IActionResult> EditFile(int fileId, IFormFile uploadedFile, string fileName, int profileId)
        {
            if (profileId <= 0)
                return BadRequest();

            FileModel files = await _unitOfWork.FileModelRepository.EditFile(fileId, uploadedFile, fileName, profileId);
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
