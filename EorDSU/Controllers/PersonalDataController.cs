using EorDSU.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Controllers
{
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

        [Route("GetPersDepartmentById")]
        [HttpGet]
        public IActionResult GetPersDepartmentById(int kafedraId)
        {
            return Ok(_unitOfWork.BasePersonActiveData.GetPersDepartmentById(kafedraId));
        }

        [Route("GetPersDepartmentByDivisionId")]
        [HttpGet]
        public IActionResult GetPersDepartmentByDivisionId(int facultyId)
        {
            return Ok(_unitOfWork.BasePersonActiveData.GetPersDepartmentByDivisionId(facultyId));
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

        [Route("GetPrepods")]
        [HttpGet]
        public IActionResult GetPrepods()
        {
            var prepods = _unitOfWork.BasePersonActiveData.GetPrepods().ToList().DistinctBy(x => x.IdСотрудника).Take(10);
            //var prepodsDistinct = prepods.DistinctBy(x => x.IdСотрудника);
            return Ok(prepods);
        }
    }
}
