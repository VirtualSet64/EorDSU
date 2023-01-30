using EorDSU.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EorDSU.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FileRPDController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FileRPDController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Создание РПД
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="disciplineId"></param>
        /// <returns></returns>
        [Route("CreateRPD")]
        [HttpPost]
        public async Task<IActionResult> CreateRPD(IFormFile uploadedFile, int disciplineId)
        {
            var rpd = await _unitOfWork.FileRPDRepository.CreateFileRPD(uploadedFile, disciplineId);

            if (rpd == null)
                return BadRequest();
            return Ok(rpd);
        }

        /// <summary>
        /// Изменение файла РПД
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="fileId"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        [Route("EditRPD")]
        [HttpPut]
        public async Task<IActionResult> EditRPD(IFormFile uploadedFile, int disciplineId)
        {
            if (disciplineId == 0)
                return BadRequest();
            var rpd = await _unitOfWork.FileRPDRepository.EditFileRPD(uploadedFile, disciplineId);
            return Ok(rpd);
        }

        /// <summary>
        /// Удаление файла РПД
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [Route("DeleteRPD")]
        [HttpDelete]
        public async Task<IActionResult> DeleteRPD(int fileRPDId)
        {
            await _unitOfWork.FileRPDRepository.Remove(fileRPDId);
            return Ok();
        }
    }
}
