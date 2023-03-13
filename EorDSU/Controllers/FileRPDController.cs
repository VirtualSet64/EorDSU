using EorDSU.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EorDSU.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FileRPDController : Controller
    {
        private readonly IFileRPDRepository _fileRPDRepository;

        public FileRPDController(IFileRPDRepository fileRPDRepository)
        {
            _fileRPDRepository = fileRPDRepository;
        }

        /// <summary>
        /// Создание РПД
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="authorId"></param>
        /// <param name="disciplineId"></param>
        /// <param name="ecp">Код ЭЦП</param>
        /// <returns></returns>
        [Route("CreateRPD")]
        [HttpPost]
        public async Task<IActionResult> CreateRPD(IFormFile uploadedFile, int authorId, int disciplineId, string? ecp)
        {
            var rpd = await _fileRPDRepository.CreateFileRPD(uploadedFile, authorId, disciplineId, ecp);

            if (rpd == null)
                return BadRequest("Ошибка добавления файла");
            return Ok();
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
            await _fileRPDRepository.Remove(fileRPDId);
            return Ok();
        }
    }
}
