using EorDSU.Common;
using EorDSU.Common.Interfaces;
using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;
using EorDSU.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sentry;
using System.IO;

namespace EorDSU.Controllers
{
    [Authorize]
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

            await _unitOfWork.DisciplineRepository.Create(discipline);
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

            await _unitOfWork.DisciplineRepository.Update(discipline);
            return Ok();
        }

        ///// <summary>
        ///// Удаление файла
        ///// </summary>
        ///// <param name="fileId"></param>
        ///// <returns></returns>
        //[Route("DeleteDiscipline")]
        //[HttpDelete]
        //public async Task<IActionResult> DeleteDiscipline(int disciplineId)
        //{
        //    if (await _disciplineRepository.RemoveDiscipline(disciplineId) == null)
        //        return BadRequest();
        //    return Ok();
        //}
    }
}
