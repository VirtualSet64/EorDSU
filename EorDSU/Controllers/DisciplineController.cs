using DomainServices.Models;
using Ifrastructure.Repository.InterfaceRepository;
using DomainServices.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DisciplineController : Controller
    {
        private readonly IDisciplineRepository _disciplineRepository;
        private readonly IUmuAndFacultyRepository _umuAndFacultyRepository;

        public DisciplineController(IDisciplineRepository disciplineRepository, IUmuAndFacultyRepository umuAndFacultyRepository)
        {
            _disciplineRepository = disciplineRepository;
            _umuAndFacultyRepository = umuAndFacultyRepository;
        }

        /// <summary>
        /// Получение дисциплин данного профиля
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        [Route("GetDisciplineByProfileId")]
        [HttpGet]
        public IActionResult GetDisciplineByProfileId(int profileId)
        {
            DataForTableResponse responseForDiscipline = _disciplineRepository.GetDisciplinesByProfileId(profileId);
            if (responseForDiscipline == null || responseForDiscipline.Disciplines == null)
                return BadRequest("Нет дисциплин для данного профиля");
            return Ok(responseForDiscipline);
        }

        /// <summary>
        /// Получение списка дисциплин доступных для удаления
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "umu, admin")]
        [Route("GetRemovableDisciplines")]
        [HttpGet]
        public async Task<IActionResult> GetRemovableDisciplines(string userId)
        {
            List<Discipline> disciplines = new();
            var faculties = await _umuAndFacultyRepository.Get().Where(x => x.UserId == userId).ToListAsync();
            foreach (var item in faculties)
            {
                disciplines.AddRange(await _disciplineRepository.GetRemovableDiscipline(item.FacultyId));
            }

            if (disciplines != null && disciplines.Any())
                return Ok(disciplines);
            return BadRequest("Нет дисциплин доступных для удаления");
        }

        /// <summary>
        /// Создание дисциплины
        /// </summary>
        /// <param name="discipline"></param>
        /// <returns></returns>
        [Authorize]
        [Route("CreateDiscipline")]
        [HttpPost]
        public async Task<IActionResult> CreateDiscipline(Discipline discipline)
        {
            if (discipline == null)
                return BadRequest();

            if (_disciplineRepository.Get().Any(x => x.DisciplineName == discipline.DisciplineName &&
                                                           x.StatusDisciplineId == discipline.StatusDisciplineId &&
                                                           x.ProfileId == discipline.ProfileId))
                return BadRequest("Такая дисциплина существует");

            await _disciplineRepository.Create(discipline);
            return Ok();
        }

        /// <summary>
        /// Изменение дисциплины
        /// </summary>
        /// <param name="discipline"></param>
        /// <returns></returns>
        [Authorize]
        [Route("EditDiscipline")]
        [HttpPut]
        public async Task<IActionResult> EditDiscipline(Discipline discipline)
        {
            if (discipline == null)
                return BadRequest("Ошибка передачи дисциплины");

            discipline.UpdateDate = DateTime.Now;
            await _disciplineRepository.Update(discipline);
            return Ok();
        }

        /// <summary>
        /// Удаление дисциплины
        /// </summary>
        /// <param name="disciplineId"></param>
        /// <returns></returns>
        [Authorize(Roles = "umu, admin")]
        [Route("DeleteDiscipline")]
        [HttpDelete]
        public async Task<IActionResult> DeleteDiscipline(int disciplineId)
        {
            if (await _disciplineRepository.RemoveDiscipline(disciplineId) == null)
                return BadRequest("Такой дисциплины не существует");
            return Ok();
        }

        /// <summary>
        /// Удаление дисциплины
        /// </summary>
        /// <param name="disciplineId"></param>
        /// <returns></returns>
        [Authorize]
        [Route("RequestDeleteDiscipline")]
        [HttpDelete]
        public async Task<IActionResult> RequestDeleteDiscipline(int disciplineId)
        {
            await _disciplineRepository.RequestDeleteDiscipline(disciplineId);
            return Ok();
        }
    }
}
