using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;
using EorDSU.Repository;
using EorDSU.ResponseModel;
using EorDSU.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sentry;

namespace EorDSU.Controllers
{
    [Authorize(Roles = "admin")]
    [ApiController]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private readonly IActiveData _activeData;
        private readonly IConfiguration Configuration;
        private readonly ApplicationContext _applicationContext;

        public AdminController(IActiveData activeData, IConfiguration configuration, ApplicationContext applicationContext)
        {
            _activeData = activeData;
            Configuration = configuration;
            _applicationContext = applicationContext;      
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("GetFileModels")]
        [HttpGet]
        public async Task<List<FileModel>> GetFileModelsAsync()
        {
            return await _activeData.GetFileModels().ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("GetFileRPDs")]
        [HttpGet]
        public async Task<List<FileRPD>> GetFileRPDsAsync()
        {
            return await _activeData.GetFileRPDs().ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="persDepartmentId"></param>
        /// <returns></returns>
        [Route("GetDataAsync")]
        [HttpGet]
        public async Task<IActionResult> GetDataAsync(int persDepartmentId)
        {
            var profiles = await _activeData.GetProfiles().Where(x => x.PersDepartmentId == persDepartmentId).ToListAsync();
            if (profiles == null)
                return BadRequest();

            List<DataResponseForSvedenOOPDGU> dataResponseForSvedenOOPDGUs = new();
            foreach (var item in profiles)
            {
                dataResponseForSvedenOOPDGUs.Add(new()
                {
                    Profiles = item,
                    CaseCEdukind = _activeData.GetCaseCEdukind().FirstOrDefault(x => x.EdukindId == item.CaseCEdukindId),
                    CaseSDepartment = _activeData.GetCaseSDepartments().FirstOrDefault(x => x.DepartmentId == item.CaseSDepartmentId),
                    SrokDeystvGosAccred = Configuration["SrokDeystvGosAccred"],
                });
            }
            return Ok(dataResponseForSvedenOOPDGUs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        [Route("AddFinalFormFile")]
        [HttpPost]
        public IActionResult AddFinalFormFile(Profile profile)
        {
            if (profile != null && profile.LevelEdu != null && profile.Disciplines != null)
            {
                _applicationContext.LevelEdues.Add(profile.LevelEdu);
                _applicationContext.Profiles.Add(profile);
                _applicationContext.Disciplines.AddRange(profile.Disciplines);
                _applicationContext.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
    }
}
