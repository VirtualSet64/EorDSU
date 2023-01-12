using BasePersonDBService.Interfaces;
using DSUContextDBService.Interfaces;
using EorDSU.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DekanatDataController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DekanatDataController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Route("GetCaseSDepartments")]
        [HttpGet]
        public async Task<IActionResult> GetCaseSDepartments()
        {
            return Ok(await _unitOfWork.DSUActiveData.GetCaseSDepartments().ToListAsync());
        }

        [Route("GetCaseSDepartmentById")]
        [HttpGet]
        public IActionResult GetCaseSDepartmentById(int kafedraId)
        {
            return Ok(_unitOfWork.DSUActiveData.GetCaseSDepartmentById(kafedraId));
        }
    }
}
