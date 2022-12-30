using BasePersonDBService.Interfaces;
using EorDSU.DBService;
using EorDSU.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DataController : Controller
    {
        private readonly IBasePersonActiveData _basePersonActiveData;
        public DataController(IBasePersonActiveData basePersonActiveData)
        {
            _basePersonActiveData = basePersonActiveData;
        }

        [Route("GetAllKafedra")]
        [HttpGet]
        public async Task<IActionResult> GetPersDepartments()
        {
            return Ok(await _basePersonActiveData.GetPersDepartments().ToListAsync());
        }

        [Route("GetKafedra")]
        [HttpGet]
        public IActionResult GetPersDepartments(int kafedraId)
        {
            return Ok(_basePersonActiveData.GetPersDepartmentById(kafedraId));
        }

        [Route("GetAllFaculty")]
        [HttpGet]
        public async Task<IActionResult> GetPersDivisions()
        {
            return Ok(await _basePersonActiveData.GetPersDivisions().ToListAsync());
        }

        [Route("GetFaculty")]
        [HttpGet]
        public IActionResult GetPersDivision(int facultyId)
        {
            return Ok(_basePersonActiveData.GetPersDivisionById(facultyId));
        }
    }
}
