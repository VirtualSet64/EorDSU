using EorDSU.Common.Interfaces;
using EorDSU.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [Route("GetStatusDiscipline")]
        [HttpGet]
        public async Task<IActionResult> GetStatusDiscipline()
        {
            List<StatusDiscipline> statusDisciplines = await _unitOfWork.StatusDisciplineRepository.GetStatusDiscipline().ToListAsync();
            if (statusDisciplines == null)
                return BadRequest();
            return Ok(statusDisciplines);
        }

        [Route("GetStatusDisciplineById")]
        [HttpGet]
        public IActionResult GetStatusDiscipline(int statusDisciplineId)
        {
            var statusDiscipline = _unitOfWork.StatusDisciplineRepository.GetStatusDisciplineById(statusDisciplineId);
            if (statusDiscipline == null)
                return BadRequest();
            return Ok(statusDiscipline);
        }

        [Route("CreateStatusDiscipline")]
        [HttpPost]
        public async Task<IActionResult> CreateStatusDiscipline(StatusDiscipline statusDiscipline)
        {
            if (statusDiscipline == null)
                return BadRequest();

            await _unitOfWork.StatusDisciplineRepository.Create(statusDiscipline);
            return Ok(); 
        }

        [Route("UpdateStatusDiscipline")]
        [HttpPut]
        public async Task<IActionResult> EditStatusDiscipline(StatusDiscipline statusDiscipline)
        {
            if (statusDiscipline == null)
                return BadRequest();

            await _unitOfWork.StatusDisciplineRepository.Update(statusDiscipline);
            return Ok();
        }

        [Route("DeleteStatusDiscipline")]
        [HttpDelete]
        public async Task<IActionResult> DeleteStatusDiscipline(int statusDisciplineId)
        {
            await _unitOfWork.StatusDisciplineRepository.RemoveStatusDiscipline(statusDisciplineId);
            return Ok();
        }
    }
}
