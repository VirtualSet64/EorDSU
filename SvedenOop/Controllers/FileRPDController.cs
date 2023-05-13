using DomainServices.Entities;
using SvedenOop.Services.Interfaces;
using Infrastructure.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SvedenOop.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FileRPDController : Controller
    {
        private readonly IFileRPDRepository _fileRPDRepository;
        private readonly IAddFileOnServer _addFileOnServer;
        private readonly IGenerateJsonService _generateJsonService;

        public FileRPDController(IFileRPDRepository fileRPDRepository, IAddFileOnServer addFileOnServer, IGenerateJsonService generateJsonService)
        {
            _fileRPDRepository = fileRPDRepository;
            _addFileOnServer = addFileOnServer;
            _generateJsonService = generateJsonService;
        }

        /// <summary>
        /// Создание РПД
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="authorId"></param>
        /// <param name="disciplineId"></param>
        /// <returns></returns>
        [Route("CreateRPD")]
        [HttpPost]
        public async Task<IActionResult> CreateRPD(IFormFile uploadedFile, int authorId, int disciplineId)
        {
            FileRPD fileRPD = new()
            {
                DisciplineId = disciplineId,
                Name = uploadedFile.FileName,
                PersonId = authorId
            };
            if (_fileRPDRepository.Get().Any(x => x.Name == uploadedFile.FileName))
                return BadRequest("Файл с таким названием уже существует");

            await _addFileOnServer.CreateFile(uploadedFile);

            await _fileRPDRepository.Create(fileRPD);

            new Task(() => _generateJsonService.GenerateJsonFile());
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
            new Task(() => _generateJsonService.GenerateJsonFile());
            return Ok();
        }
    }
}
