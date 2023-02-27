using EorDSU.Common.Interfaces;
using EorDSU.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EorDSU.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class StatusDisciplineController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StatusDisciplineController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Получение статусов дисциплин
        /// </summary>
        /// <returns></returns>
        [Route("GetStatusDiscipline")]
        [HttpGet]
        public IActionResult GetStatusDiscipline()
        {
            List<StatusDiscipline> statusDisciplines = _unitOfWork.StatusDisciplineRepository.GetStatusDiscipline();
            if (statusDisciplines.Any())
                return Ok(statusDisciplines);
            return BadRequest("Нет созданных статусов дисциплин");
        }

        /// <summary>
        /// Получение списка статусов дисциплин доступных для удаления
        /// </summary>
        /// <returns></returns>
        [Route("GetRemovableStatusDiscipline")]
        [HttpGet]
        public IActionResult GetRemovableStatusDiscipline()
        {
            List<StatusDiscipline> statusDisciplines = _unitOfWork.StatusDisciplineRepository.GetRemovableStatusDiscipline();
            if (statusDisciplines.Any())
                return Ok(statusDisciplines);
            return BadRequest("Нет статусов дисциплин доступных для удаления");            
        }

        /// <summary>
        /// Получение статуса дисциплины по id
        /// </summary>
        /// <param name="statusDisciplineId"></param>
        /// <returns></returns>
        [Route("GetStatusDisciplineById")]
        [HttpGet]
        public IActionResult GetStatusDisciplineById(int statusDisciplineId)
        {
            var statusDiscipline = _unitOfWork.StatusDisciplineRepository.GetStatusDisciplineById(statusDisciplineId);
            if (statusDiscipline == null)
                return BadRequest("Статус дисциплины не найден");
            return Ok(statusDiscipline);
        }

        /// <summary>
        /// Создание статуса дисциплины
        /// </summary>
        /// <param name="statusDiscipline"></param>
        /// <returns></returns>
        [Route("CreateStatusDiscipline")]
        [HttpPost]
        public async Task<IActionResult> CreateStatusDiscipline(StatusDiscipline statusDiscipline)
        {
            if (statusDiscipline == null)
                return BadRequest("Ошибка при передаче статуса дисциплины");

            await _unitOfWork.StatusDisciplineRepository.Create(statusDiscipline);
            return Ok(); 
        }

        /// <summary>
        /// Изменение статуса дисциплины
        /// </summary>
        /// <param name="statusDiscipline"></param>
        /// <returns></returns>
        [Route("UpdateStatusDiscipline")]
        [HttpPut]
        public async Task<IActionResult> EditStatusDiscipline(StatusDiscipline statusDiscipline)
        {
            if (statusDiscipline == null)
                return BadRequest("Ошибка при передаче статуса дисциплины");

            await _unitOfWork.StatusDisciplineRepository.Update(statusDiscipline);
            return Ok();
        }

        [Authorize(Roles = "admin")]
        /// <summary>
        /// Удаление статуса дисциплины
        /// </summary>
        /// <param name="statusDisciplineId"></param>
        /// <returns></returns>
        [Route("DeleteStatusDiscipline")]
        [HttpDelete]
        public async Task<IActionResult> DeleteStatusDiscipline(int statusDisciplineId)
        {
            await _unitOfWork.StatusDisciplineRepository.RemoveStatusDiscipline(statusDisciplineId);
            return Ok();
        }
    }
}
