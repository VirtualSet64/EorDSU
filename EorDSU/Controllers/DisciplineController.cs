using EorDSU.Common.Interfaces;
using EorDSU.Models;
using EorDSU.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EorDSU.Controllers
{
    //[Authorize]
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
            ResponseForDiscipline responseForDiscipline = _unitOfWork.DisciplineRepository.GetDisciplinesByProfileId(profileId);
            if (responseForDiscipline.Disciplines.Any())
                return Ok(responseForDiscipline);
            return BadRequest("Нет дисциплин для данного профиля");            
        }

        /// <summary>
        /// Создание дисциплины
        /// </summary>
        /// <param name="discipline"></param>
        /// <returns></returns>
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
        [Route("DeleteDiscipline")]
        [HttpDelete]
        public async Task<IActionResult> DeleteDiscipline(int disciplineId)
        {
            if (await _unitOfWork.DisciplineRepository.RemoveDiscipline(disciplineId) == null)
                return BadRequest("Такой дисциплины не существует");
            return Ok();
        }
    }
}
