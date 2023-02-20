using EorDSU.Common.Interfaces;
using EorDSU.Models;
using EorDSU.ViewModels;
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
        /// <param name="kafedraId"></param>
        /// <returns></returns>
        [Route("GetDataById")]
        [HttpGet]
        public async Task<IActionResult> GetDataByKafedraId(int kafedraId)
        {
            return Ok(await _unitOfWork.ProfileRepository.GetData(kafedraId));
        }

        /// <summary>
        /// Получение всех данных факультета
        /// </summary>
        /// <param name="facultyId"></param>
        /// <returns></returns>
        [Route("GetDataFacultyById")]
        [HttpGet]
        public async Task<IActionResult> GetDataByFacultyId(int facultyId)
        {
            return Ok(await _unitOfWork.ProfileRepository.GetDataByFacultyId(facultyId));
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
                return BadRequest("Ошибка передачи профиля");

            if (_unitOfWork.ProfileRepository.Get().Any(x => x.ProfileName == profile.ProfileName &&
                             x.TermEdu == profile.TermEdu &&
                             x.CaseCEdukindId == profile.CaseCEdukindId &&
                             x.CaseSDepartmentId == profile.CaseSDepartmentId &&
                             x.LevelEdu == profile.LevelEdu &&
                             x.Year == profile.Year))
                return BadRequest("Такой профиль уже существует");

            profile.CreateDate = DateTime.Now;
            await _unitOfWork.ProfileRepository.Create(profile);
            return Ok();
        }

        /// <summary>
        /// Создание профиля
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <returns></returns>
        //[Authorize]
        [Route("ParsingProfileByFile")]
        [HttpPost]
        public async Task<IActionResult> ParsingProfileByFile(IFormFile uploadedFile)
        {
            if (uploadedFile == null)
                return BadRequest("Ошибка передачи файла");

            DataResponseForSvedenOOPDGU profile = await _unitOfWork.ProfileRepository.ParsingProfileByFile(uploadedFile);
            return Ok(profile);
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
                return BadRequest("Ошибка передачи профиля");

            profile.UpdateDate = DateTime.Now;
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
            try
            {
                await _unitOfWork.ProfileRepository.RemoveProfile(profileId);
                return Ok();
            }
            catch
            {
                return BadRequest("Профиль не найден");
                throw;
            }
        }
    }
}
