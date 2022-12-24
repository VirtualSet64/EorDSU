using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace EorDSU.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DisciplineController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IApplicationActiveData _activeData;

        public DisciplineController(ApplicationContext context, IApplicationActiveData activeData)
        {
            _context = context;
            _activeData = activeData;
        }

        /// <summary>
        /// Получение всех дисциплин данного профиля
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        [Route("GetDisciplines")]
        [HttpGet]
        public IActionResult GetDisciplines(int profileId)
        {
            return Ok(_activeData.GetDisciplines().Where(x => x.ProfileId == profileId).ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="discipline"></param>
        /// <returns></returns>
        [Route("CreateDiscipline")]
        [HttpPost]
        public async Task<IActionResult> CreateDiscipline(Discipline discipline)
        {
            if (discipline == null)
                return BadRequest();

            await _context.Disciplines.AddAsync(discipline);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="discipline"></param>
        /// <returns></returns>
        [Route("EditDiscipline")]
        [HttpPut]
        public async Task<IActionResult> EditDiscipline(Discipline discipline)
        {
            if (discipline == null)
                return BadRequest();

            _context.Disciplines.Update(discipline);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [Route("DeleteDiscipline")]
        [HttpDelete]
        public async Task<IActionResult> DeleteDiscipline(int disciplineId)
        {
            var discipline = _context.Disciplines.FirstOrDefault(x => x.Id == disciplineId);

            if (discipline == null)
                return BadRequest();

            _context.Disciplines.Remove(discipline);
            await _context.SaveChangesAsync();
            return Ok();
        }        
    }
}
