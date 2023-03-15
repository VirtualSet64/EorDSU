using DomainServices.Entities;
using Ifrastructure.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EorDSU.Controllers
{
    [Authorize(Roles ="admin")]
    [ApiController]
    [Route("[controller]")]
    public class FileTypeController : Controller
    {
        private readonly IFileTypeRepository _fileTypeRepository;

        public FileTypeController(IFileTypeRepository fileTypeRepository)
        {
            _fileTypeRepository = fileTypeRepository;
        }

        [Route("GetFileTypes")]
        [HttpGet]
        public IActionResult GetFileTypes()
        {
            return Ok(_fileTypeRepository.Get());
        }

        [Route("CreateFileType")]
        [HttpPost]
        public async Task<IActionResult> CreateFileType(FileType fileType)
        {
            await _fileTypeRepository.Create(fileType);
            return Ok();
        }

        [Route("EditFileType")]
        [HttpPut]
        public async Task<IActionResult> EditFileType(FileType fileType)
        {
            await _fileTypeRepository.Update(fileType);
            return Ok();
        }

        [Route("DeleteFileType")]
        [HttpDelete]
        public async Task<IActionResult> DeleteFileType(int fileTypeId)
        {
            await _fileTypeRepository.Remove(fileTypeId);
            return Ok();
        }
    }
}
