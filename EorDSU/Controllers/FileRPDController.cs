using DomainServices.DtoModels;
using EorDSU.Services.Interfaces;
using Ifrastructure.Repository.InterfaceRepository;
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
        private readonly IAddFileOnServer _addFileOnServer;

        public FileRPDController(IFileRPDRepository fileRPDRepository, IAddFileOnServer addFileOnServer)
        {
            _fileRPDRepository = fileRPDRepository;
            _addFileOnServer = addFileOnServer;
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
        public async Task<IActionResult> CreateRPD(UploadFileRPD uploadedFile)
        {
            if (_fileRPDRepository.Get().Any(x => x.Name == uploadedFile.UploadedFile.FileName))
                return BadRequest("Файл с таким названием уже существует");

            await _addFileOnServer.CreateFile(uploadedFile.UploadedFile);
            await _fileRPDRepository.CreateFileRPD(uploadedFile);

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
