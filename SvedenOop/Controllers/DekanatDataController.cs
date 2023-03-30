using BasePersonDBService.Interfaces;
using DSUContextDBService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SvedenOop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DekanatDataController : Controller
    {
        private readonly IDSUActiveData _dSUActiveData;
        private readonly IBasePersonActiveData _basePersonActiveData;

        public DekanatDataController(IDSUActiveData dSUActiveData, IBasePersonActiveData basePersonActiveData)
        {
            _basePersonActiveData = basePersonActiveData;
            _dSUActiveData = dSUActiveData;
        }

        [Route("GetCaseSDepartments")]
        [HttpGet]
        public async Task<IActionResult> GetCaseSDepartments()
        {
            return Ok(await _dSUActiveData.GetCaseSDepartments().ToListAsync());
        }

        /// <summary>
        /// Получение списка направлений по id кафедры
        /// </summary>
        /// <param name="kafedraId"></param>
        /// <returns></returns>
        [Route("GetCaseSDepartmentByKafedraId")]
        [HttpGet]
        public async Task<IActionResult> GetCaseSDepartmentByIdAsync(int kafedraId)
        {
            int? facultyId = _basePersonActiveData.GetPersDepartmentById(kafedraId).DivId;
            if (facultyId == null)
                return BadRequest("Не существует такой кафедры или связанного факультета");
            return Ok(await _dSUActiveData.GetCaseSDepartmentByFacultyId(facultyId).ToListAsync());
        }

        [Route("GetCaseSEdukinds")]
        [HttpGet]
        public async Task<IActionResult> GetCaseSEdukinds()
        {
            return Ok(await _dSUActiveData.GetCaseCEdukinds().ToListAsync());
        }
    }
}
