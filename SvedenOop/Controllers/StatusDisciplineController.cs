using DomainServices.Entities;
using Ifrastructure.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EorDSU.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class StatusDisciplineController : Controller
    {
        private readonly IStatusDisciplineRepository _statusDisciplineRepository;

        public StatusDisciplineController(IStatusDisciplineRepository statusDisciplineRepository)
        {
            _statusDisciplineRepository = statusDisciplineRepository;
        }

        /// <summary>
        /// Получение статусов дисциплин
        /// </summary>
        /// <returns></returns>
        [Route("GetStatusDiscipline")]
        [HttpGet]
        public IActionResult GetStatusDiscipline()
        {
            List<StatusDiscipline> statusDisciplines = _statusDisciplineRepository.GetStatusDiscipline();
            if (statusDisciplines.Any())
                return Ok(statusDisciplines);
            return BadRequest("Нет созданных статусов дисциплин");
        }

        /// <summary>
        /// Получение списка статусов дисциплин доступных для удаления
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "umu, admin")]
        [Route("GetRemovableStatusDiscipline")]
        [HttpGet]
        public async Task<IActionResult> GetRemovableStatusDiscipline()
        {
            List<StatusDiscipline> statusDisciplines = await _statusDisciplineRepository.GetRemovableStatusDiscipline();
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
            var statusDiscipline = _statusDisciplineRepository.FindById(statusDisciplineId);
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

            await _statusDisciplineRepository.Create(statusDiscipline);
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

            await _statusDisciplineRepository.Update(statusDiscipline);
            return Ok();
        }
                
        /// <summary>
        /// Удаление статуса дисциплины
        /// </summary>
        /// <param name="statusDisciplineId"></param>
        /// <returns></returns>
        [Authorize(Roles = "umu, admin")]
        [Route("DeleteStatusDiscipline")]
        [HttpDelete]
        public async Task<IActionResult> DeleteStatusDiscipline(int statusDisciplineId)
        {
            await _statusDisciplineRepository.RemoveStatusDiscipline(statusDisciplineId);
            return Ok();
        }

        /// <summary>
        /// Запрос на удаление статуса дисциплины
        /// </summary>
        /// <param name="statusDisciplineId"></param>
        /// <returns></returns>
        [Route("RequestDeleteStatusDiscipline")]
        [HttpDelete]
        public async Task<IActionResult> RequestDeleteStatusDiscipline(int statusDisciplineId)
        {
            await _statusDisciplineRepository.RequestDeleteStatusDiscipline(statusDisciplineId);
            return Ok();
        }
    }
}
