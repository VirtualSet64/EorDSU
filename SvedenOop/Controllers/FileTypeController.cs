using DomainServices.Entities;
using Infrastructure.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SvedenOop.Services.Interfaces;

namespace SvedenOop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileTypeController : Controller
    {
        private readonly IFileTypeRepository _fileTypeRepository;
        private readonly IGenerateJsonService _generateJsonService;

        public FileTypeController(IFileTypeRepository fileTypeRepository, IGenerateJsonService generateJsonService)
        {
            _fileTypeRepository = fileTypeRepository;
            _generateJsonService = generateJsonService;
        }

        [Route("GetFileTypes")]
        [HttpGet]
        public IActionResult GetFileTypes()
        {
            return Ok(_fileTypeRepository.Get());
        }

        [Authorize(Roles = "admin")]
        [Route("CreateFileType")]
        [HttpPost]
        public async Task<IActionResult> CreateFileType(FileType fileType)
        {
            await _fileTypeRepository.Create(fileType);
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [Route("EditFileType")]
        [HttpPut]
        public async Task<IActionResult> EditFileType(FileType fileType)
        {
            await _fileTypeRepository.Update(fileType);
            new Task(() => _generateJsonService.GenerateJsonFile());
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [Route("DeleteFileType")]
        [HttpDelete]
        public async Task<IActionResult> DeleteFileType(int fileTypeId)
        {
            try
            {
                await _fileTypeRepository.Remove(fileTypeId);
            }
            catch (Exception)
            {
                return BadRequest("Невозможно удалить данный тип файла из-за дочерних объектов");
                throw;
            }

            return Ok();
        }
    }
}
