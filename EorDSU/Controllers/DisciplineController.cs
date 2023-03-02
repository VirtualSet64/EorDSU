using EorDSU.Common.Interfaces;
using EorDSU.Models;
using EorDSU.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DisciplineController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DisciplineController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            DataForTableResponse responseForDiscipline = _unitOfWork.DisciplineRepository.GetDisciplinesByProfileId(profileId);
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
            var faculties = await _unitOfWork.UmuAndFacultyRepository.Get().Where(x => x.UserId == userId).ToListAsync();
            foreach (var item in faculties)
            {
                disciplines.AddRange(await _unitOfWork.DisciplineRepository.GetRemovableDiscipline(item.FacultyId));
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

            if (_unitOfWork.DisciplineRepository.Get().Any(x => x.DisciplineName == discipline.DisciplineName &&
                                                           x.StatusDisciplineId == discipline.StatusDisciplineId &&
                                                           x.ProfileId == discipline.ProfileId))
                return BadRequest("Такая дисциплина существует");

            discipline.CreateDate = DateTime.Now;
            await _unitOfWork.DisciplineRepository.Create(discipline);
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
            await _unitOfWork.DisciplineRepository.Update(discipline);
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
            if (await _unitOfWork.DisciplineRepository.RemoveDiscipline(disciplineId) == null)
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
        public IActionResult RequestDeleteDiscipline(int disciplineId)
        {
            _unitOfWork.DisciplineRepository.RequestDeleteDiscipline(disciplineId);
            return Ok();
        }
    }
}
