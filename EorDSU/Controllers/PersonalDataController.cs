using EorDSU.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PersonalDataController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PersonalDataController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Route("GetAllKafedra")]
        [HttpGet]
        public async Task<IActionResult> GetPersDepartments()
        {
            return Ok(await _unitOfWork.BasePersonActiveData.GetPersDepartments().ToListAsync());
        }

        [Route("GetKafedra")]
        [HttpGet]
        public IActionResult GetPersDepartments(int kafedraId)
        {
            return Ok(_unitOfWork.BasePersonActiveData.GetPersDepartmentById(kafedraId));
        }

        [Route("GetAllFaculty")]
        [HttpGet]
        public async Task<IActionResult> GetPersDivisions()
        {
            return Ok(await _unitOfWork.BasePersonActiveData.GetPersDivisions().ToListAsync());
        }

        [Route("GetFaculty")]
        [HttpGet]
        public IActionResult GetPersDivision(int facultyId)
        {
            return Ok(_unitOfWork.BasePersonActiveData.GetPersDivisionById(facultyId));
        }
    }
}
