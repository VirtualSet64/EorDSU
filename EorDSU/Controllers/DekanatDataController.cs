using EorDSU.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Controllers
{
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

        [Route("GetCaseSDepartmentByKafedraId")]
        [HttpGet]
        public async Task<IActionResult> GetCaseSDepartmentByIdAsync(int kafedraId)
        {
            int? facultyId = _unitOfWork.BasePersonActiveData.GetPersDepartmentById(kafedraId).DivId;
            if (facultyId == null)
                return BadRequest("Не существует такой кафедры или связанного факультета");
            return Ok(await _unitOfWork.DSUActiveData.GetCaseSDepartmentByFacultyId(facultyId).ToListAsync());
        }

        [Route("GetCaseSEdukinds")]
        [HttpGet]
        public async Task<IActionResult> GetCaseSEdukinds()
        {
            return Ok(await _unitOfWork.DSUActiveData.GetCaseCEdukinds().ToListAsync());
        }
    }
}
