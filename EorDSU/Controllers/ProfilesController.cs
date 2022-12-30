using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sentry;

namespace EorDSU.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProfilesController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IApplicationActiveData _activeData;

        public ProfilesController(ApplicationContext context, IApplicationActiveData activeData)
        {
            _context = context;
            _activeData = activeData;
        }

        [Route("GetProfiles")]
        [HttpGet]
        public IActionResult GetProfiles()
        {
            return Ok(_activeData.GetProfiles().ToList());
        }

        [Route("GetProfileById")]
        [HttpGet]
        public IActionResult GetProfileById(int profileId)
        {
            return Ok(_activeData.GetProfileById(profileId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="discipline"></param>
        /// <returns></returns>
        [Route("CreateDiscipline")]
        [HttpPost]
        public async Task<IActionResult> CreateProfile(Profile profile)
        {
            if (profile == null)
                return BadRequest();

            await _context.Profiles.AddAsync(profile);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="discipline"></param>
        /// <returns></returns>
        [Route("EditDiscipline")]
        [HttpPut]
        public async Task<IActionResult> EditDiscipline(Profile profile)
        {
            if (profile == null)
                return BadRequest();

            _context.Profiles.Update(profile);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [Route("DeleteDiscipline")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProfile(int profileId)
        {
            var profile = await _activeData.GetProfileById(profileId);

            if (profile == null)
                return BadRequest();

            if (profile.Disciplines != null)
                _context.Disciplines.RemoveRange(profile.Disciplines);

            if (profile.FileModels != null)
                _context.FileModels.RemoveRange(profile.FileModels);

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
