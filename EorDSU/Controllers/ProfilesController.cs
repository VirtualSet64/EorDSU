﻿using DomainServices.Models;
using Ifrastructure.Repository.InterfaceRepository;
using DomainServices.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EorDSU.Services.Interfaces;
using EorDSU.Services;

namespace EorDSU.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfilesController : Controller
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IDisciplineRepository _disciplineRepository;
        private readonly IAddFileOnServer _addFileOnServer;

        public ProfilesController(IProfileRepository profileRepository, IDisciplineRepository disciplineRepository, IAddFileOnServer addFileOnServer)
        {
            _profileRepository = profileRepository;
            _disciplineRepository = disciplineRepository;
            _addFileOnServer = addFileOnServer;
        }

        /// <summary>
        /// Получение всех данных 
        /// </summary>
        /// <returns></returns>
        [Route("GetData")]
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var profileDto = await _profileRepository.GetData();
            profileDto.ForEach(x => x.Disciplines = _disciplineRepository.GetDisciplinesByProfileId(x.Profile.Id).Disciplines?.Where(x => x.Code?.Contains("Б2") == true).ToList());
            return Ok(profileDto);
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
            var profileDto = await _profileRepository.GetDataByKafedraId(kafedraId);
            profileDto.ForEach(x => x.Disciplines = _disciplineRepository.GetDisciplinesByProfileId(x.Profile.Id).Disciplines?.Where(x => x.Code?.Contains("Б2") == true).ToList());
            return Ok(profileDto);
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
            var profileDto = await _profileRepository.GetDataByFacultyId(facultyId);
            profileDto.ForEach(x => x.Disciplines = _disciplineRepository.GetDisciplinesByProfileId(x.Profile.Id).Disciplines?.Where(x => x.Code?.Contains("Б2") == true).ToList());
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
            return Ok();
        }

        /// <summary>
        /// Создание профиля
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

            profile.UpdateDate = DateTime.Now;
            await _profileRepository.Update(profile);
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
