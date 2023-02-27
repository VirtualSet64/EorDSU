using EorDSU.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EorDSU.Controllers
{
    [Authorize]
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
        /// <param name="ecp">Код ЭЦП</param>
        /// <returns></returns>
        [Route("CreateRPD")]
        [HttpPost]
        public async Task<IActionResult> CreateRPD(IFormFile uploadedFile, int disciplineId, string? ecp)
        {
            var rpd = await _unitOfWork.FileRPDRepository.CreateFileRPD(uploadedFile, disciplineId, ecp);

            if (rpd == null)
                return BadRequest("Ошибка добавления файла");
            return Ok(rpd);
        }

        /// <summary>
        /// Удаление файла РПД
        /// </summary>
        /// <param name="fileRPDId"></param>
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
