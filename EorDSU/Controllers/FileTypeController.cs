using EorDSU.Common.Interfaces;
using EorDSU.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EorDSU.Controllers
{
    [Authorize(Roles ="admin")]
    [ApiController]
    [Route("[controller]")]
    public class FileTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FileTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Route("GetFileTypes")]
        [HttpGet]
        public IActionResult GetFileTypes()
        {
            return Ok(_unitOfWork.FileTypeRepository.Get());
        }

        [Route("CreateFileType")]
        [HttpPost]
        public async Task<IActionResult> CreateFileType(FileType fileType)
        {
            await _unitOfWork.FileTypeRepository.Create(fileType);
            return Ok();
        }

        [Route("EditFileType")]
        [HttpPut]
        public async Task<IActionResult> EditFileType(FileType fileType)
        {
            await _unitOfWork.FileTypeRepository.Update(fileType);
            return Ok();
        }

        [Route("DeleteFileType")]
        [HttpDelete]
        public async Task<IActionResult> DeleteFileType(int fileTypeId)
        {
            await _unitOfWork.FileTypeRepository.Remove(fileTypeId);
            return Ok();
        }
    }
}
