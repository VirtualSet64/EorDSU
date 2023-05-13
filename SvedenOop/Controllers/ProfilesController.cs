using DomainServices.Entities;
using Infrastructure.Repository.InterfaceRepository;
using DomainServices.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SvedenOop.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SvedenOop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfilesController : Controller
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IDisciplineRepository _disciplineRepository;
        private readonly IAddFileOnServer _addFileOnServer;
        private readonly IGenerateJsonService _generateJsonService;
        public ProfilesController(IProfileRepository profileRepository, IDisciplineRepository disciplineRepository, IAddFileOnServer addFileOnServer, IGenerateJsonService generateJsonService)
        {
            _profileRepository = profileRepository;
            _disciplineRepository = disciplineRepository;
            _addFileOnServer = addFileOnServer;
            _generateJsonService = generateJsonService;
        }

        /// <summary>
        /// Получение всех данных для таблицы /Sveden/Oop_Dgu
        /// </summary>
        /// <returns></returns>
        [Route("GetDataForOopDgu")]
        [HttpGet]
        public IActionResult GetDataForOopDgu()
        {
            string text = _generateJsonService.GetGeneratedJsonFile();
            return Ok(text);
        }

        /// <summary>
        /// Получение всех данных для таблицы /Sveden/Opop2
        /// </summary>
        /// <returns></returns>
        [Route("GetDataOpop2")]
        [HttpGet]
        public IActionResult GetDataOpop2()
        {
            var profileDto = _profileRepository.GetDataOpop2();
            profileDto.ForEach(x => x.Disciplines = _disciplineRepository.Get().Include(d => d.FileRPD).Where(c => c.ProfileId == x.Profile.Id && c.Code.Contains("Б2") == true).ToList());

            return Ok(profileDto);
        }

        /// <summary>
        /// Получение всех данных кафедры
        /// </summary>
        /// <param name="kafedraId"></param>
        /// <returns></returns>
        [Route("GetDataByKafedraId")]
        [HttpGet]
        public IActionResult GetDataByKafedraId(int kafedraId)
        {
            var profileDto = _profileRepository.GetDataByKafedraId(kafedraId);
            profileDto.ForEach(x => x.Disciplines = _disciplineRepository.Get().Include(d => d.FileRPD).Where(c => c.ProfileId == x.Profile.Id && c.Code.Contains("Б2") == true).ToList());
            return Ok(profileDto);
        }

        /// <summary>
        /// Получение всех данных факультета
        /// </summary>
        /// <param name="facultyId"></param>
        /// <returns></returns>
        [Route("GetDataFacultyById")]
        [HttpGet]
        public IActionResult GetDataByFacultyId(int facultyId)
        {
            var profileDto = _profileRepository.GetDataByFacultyId(facultyId);
            profileDto.ForEach(x => x.Disciplines = _disciplineRepository.Get().Include(d => d.FileRPD).Where(c => c.ProfileId == x.Profile.Id && c.Code.Contains("Б2") == true).ToList());
            return Ok(profileDto);
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
            return Ok(_profileRepository.GetProfileById(profileId));
        }

        /// <summary>
        /// Создание профиля
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        [Authorize]
        [Route("CreateProfile")]
        [HttpPost]
        public async Task<IActionResult> CreateProfile(Profile profile)
        {
            if (profile == null)
                return BadRequest("Ошибка передачи профиля");

            if (_profileRepository.Get().Any(x => x.ProfileName == profile.ProfileName &&
                             x.TermEdu == profile.TermEdu &&
                             x.CaseCEdukindId == profile.CaseCEdukindId &&
                             x.CaseSDepartmentId == profile.CaseSDepartmentId &&
                             x.LevelEdu == profile.LevelEdu &&
                             x.Year == profile.Year))
                return BadRequest("Такой профиль уже существует");

            profile.Disciplines?.ForEach(x => x.StatusDiscipline = null);
            await _profileRepository.Create(profile);

            new Task(() => _generateJsonService.GenerateJsonFile());
            return Ok();
        }

        /// <summary>
        /// Парсинг учебного плана для получения данных профиля
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <returns></returns>
        [Authorize]
        [Route("ParsingProfileByFile")]
        [HttpPost]
        public async Task<IActionResult> ParsingProfileByFile(IFormFile uploadedFile)
        {
            if (uploadedFile == null)
                return BadRequest("Ошибка передачи файла");

            string path = await _addFileOnServer.CreateFile(uploadedFile);

            DataResponseForSvedenOOPDGU profile = await _profileRepository.ParsingProfileByFile(path);
            return Ok(profile);
        }

        /// <summary>
        /// Изменение профиля
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        [Authorize]
        [Route("EditProfile")]
        [HttpPut]
        public async Task<IActionResult> EditProfile(Profile profile)
        {
            if (profile == null)
                return BadRequest("Ошибка передачи профиля");

            await _profileRepository.UpdateProfile(profile);
            new Task(() => _generateJsonService.GenerateJsonFile());
            return Ok();
        }

        /// <summary>
        /// Удаление профиля
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        [Authorize]
        [Route("DeleteProfile")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProfile(int profileId)
        {
            try
            {
                await _profileRepository.RemoveProfile(profileId);
                new Task(() => _generateJsonService.GenerateJsonFile());
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Профиль не найден");
                throw;
            }
        }
    }
}
