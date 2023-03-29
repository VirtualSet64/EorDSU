using BasePersonDBService.Interfaces;
using DomainServices.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonalDataController : Controller
    {
        private readonly IBasePersonActiveData _basePersonActiveData;
        public PersonalDataController(IBasePersonActiveData basePersonActiveData)
        {
            _basePersonActiveData= basePersonActiveData;
        }

        [Route("GetAllKafedra")]
        [HttpGet]
        public async Task<IActionResult> GetPersDepartments()
        {
            return Ok(await _basePersonActiveData.GetPersDepartments().ToListAsync());
        }

        [Route("GetPersDepartmentById")]
        [HttpGet]
        public IActionResult GetPersDepartmentById(int kafedraId)
        {
            return Ok(_basePersonActiveData.GetPersDepartmentById(kafedraId));
        }

        [Route("GetPersDepartmentByDivisionId")]
        [HttpGet]
        public IActionResult GetPersDepartmentByDivisionId(int facultyId)
        {
            return Ok(_basePersonActiveData.GetPersDepartmentByDivisionId(facultyId));
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

        [Authorize]
        [Route("GetAuthors")]
        [HttpGet]
        public IActionResult GetAuthors()
        {
            var author = _basePersonActiveData.GetPrepods().ToList().DistinctBy(x => x.IdСотрудника).Select(x => new Author()
            {
                Id = x.IdСотрудника,
                Fio = x.ФИО,
                Kafedra = x.Кафедра
            });
            return Ok(author);
        }
    }
}
