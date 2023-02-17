using EorDSU.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EorDSU.Controllers
{
    //[Authorize]
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
        /// <param name="ecp">Код ЭЦП</param>
        /// <returns></returns>
        [Route("CreateFileModel")]
        [HttpPost]
        public async Task<IActionResult> CreateFileModel(List<IFormFile> uploadedFile, string fileName, int fileType, int profileId, string? ecp)
        {
            var files = await _unitOfWork.FileModelRepository.CreateFileModel(uploadedFile, fileName, fileType, profileId, ecp);
            if (files == null || !files.Any())
                return BadRequest();
            return Ok(files);
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
        public async Task<IActionResult> EditFile(int fileId, string fileName, int profileId, IFormFile? uploadedFile)
        {
            var files = await _unitOfWork.FileModelRepository.EditFile(fileId, fileName, profileId, uploadedFile);
            if (files == null)
                return BadRequest();
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
