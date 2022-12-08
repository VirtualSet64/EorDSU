using EorDSU.DBService;
using EorDSU.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : Controller
    {
        private readonly IActiveData _activeData;

        public DataController(IActiveData activeData)
        {
            _activeData = activeData;
        }

        [Route("GetAllKafedra")]
        [HttpGet]
        public async Task<IActionResult> GetPersDepartments()
        {
            return Ok(await _activeData.GetPersDepartments().ToListAsync());
        }

        [Route("GetKafedra")]
        [HttpGet]
        public async Task<IActionResult> GetPersDepartments(int kafedraId)
        {
            return Ok(await _activeData.GetPersDepartments().FirstOrDefaultAsync(x => x.DepId == kafedraId));
        }

        [Route("GetAllFaculty")]
        [HttpGet]
        public async Task<IActionResult> GetPersDivisions()
        {
            return Ok(await _activeData.GetPersDivisions().ToListAsync());
        }

        [Route("GetFaculty")]
        [HttpGet]
        public async Task<IActionResult> GetPersDivision(int facultyId)
        {
            return Ok(await _activeData.GetPersDivisions().FirstOrDefaultAsync(x => x.DivId == facultyId));
        }
    }
}
