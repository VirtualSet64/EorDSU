using EorDSU.Common;
using EorDSU.Common.Interfaces;
using EorDSU.Models;
using EorDSU.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EorDSU.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfilesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfilesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Получение всех данных 
        /// </summary>
        /// <returns></returns>
        [Route("GetData")]
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            return Ok(await _unitOfWork.ProfileRepository.GetData());
        }

        /// <summary>
        /// Получение всех данных кафедры
        /// </summary>
        /// <returns></returns>
        [Route("GetDataById")]
        [HttpGet]
        public async Task<IActionResult> GetData(int cafedraId)
        {
            return Ok(await _unitOfWork.ProfileRepository.GetData(cafedraId));
        }

        /// <summary>
        /// Получение всех данных факультета
        /// </summary>
        /// <returns></returns>
        [Route("GetDataFacultyById")]
        [HttpGet]
        public async Task<IActionResult> GetDataFacultyById(int facultyId)
        {
            return Ok(await _unitOfWork.ProfileRepository.GetDataFacultyById(facultyId));
        }

        /// <summary>
        /// Получение профиля по id
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        [Route("GetProfileById")]
        [HttpGet]
        public IActionResult GetProfileById(int profileId)
        {
            return Ok(_unitOfWork.ProfileRepository.GetProfileById(profileId));
        }

        /// <summary>
        /// Создание профиля
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        //[Authorize]
        [Route("CreateProfile")]
        [HttpPost]
        public async Task<IActionResult> CreateProfile(Profile profile)
        {
            if (profile == null)
                return BadRequest();

            await _unitOfWork.ProfileRepository.Create(profile);
            return Ok();
        }

        /// <summary>
        /// Изменение профиля
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        //[Authorize]
        [Route("EditProfile")]
        [HttpPut]
        public async Task<IActionResult> EditProfile(Profile profile)
        {
            if (profile == null)
                return BadRequest();

            await _unitOfWork.ProfileRepository.Update(profile);
            return Ok();
        }

        /// <summary>
        /// Удаление профиля
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        //[Authorize]
        [Route("DeleteProfile")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProfile(int profileId)
        {
            if (await _unitOfWork.ProfileRepository.RemoveProfile(profileId) == null)
                return BadRequest();
            return Ok();
        }
    }
}
