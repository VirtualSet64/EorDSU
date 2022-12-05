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
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private readonly IActiveData _activeData;
        private readonly ApplicationContext _applicationContext;
        private readonly DSUContext _dSUContext;
        private readonly IConfiguration Configuration;

        public AdminController(IActiveData activeData, ApplicationContext applicationContext, DSUContext dSUContext, IConfiguration configuration)
        {
            _activeData = activeData;
            _applicationContext = applicationContext;
            _dSUContext = dSUContext;
            Configuration = configuration;
        }

        [Route("GetFileModels")]
        [HttpGet]
        public async Task<List<FileModel>> GetFileModelsAsync()
        {
            return await _activeData.GetFileModels().ToListAsync();
        }

        [Route("GetFileRPDs")]
        [HttpGet]
        public async Task<List<FileRPD>> GetFileRPDsAsync()
        {
            return await _activeData.GetFileRPDs().ToListAsync();
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
            if (profile != null && profile != null && profile.LevelEdu != null && profile.Disciplines != null)
            {
                _applicationContext.LevelEdues.Add(profile.LevelEdu);
                _applicationContext.Profiles.Add(profile);
                _applicationContext.Disciplines.AddRange(profile.Disciplines);
                _applicationContext.SaveChanges();
                return Ok();
            }
            else
                return BadRequest();
        }

        [Route("GetDataAsync")]
        [HttpGet]
        public async Task<IActionResult> GetDataAsync(int persDepartmentId)
        {
            var profiles = await _activeData.GetProfiles().Where(x => x.PersDepartmentId == persDepartmentId).ToListAsync();
            List<DataResponseForSvedenOOPDGU> dataResponseForSvedenOOPDGUs = new();
            foreach (var item in profiles)
            {
                dataResponseForSvedenOOPDGUs.Add(new()
                {
                    Profiles = item,
                    CaseCEdukind = _dSUContext.CaseCEdukinds.FirstOrDefault(x => x.EdukindId == item.CaseCEdukindId),
                    CaseSDepartment = _dSUContext.CaseSDepartments.FirstOrDefault(x => x.DepartmentId == item.CaseSDepartmentId),
                    SrokDeystvGosAccred = Configuration["SrokDeystvGosAccred"],
                });
            }
            return Ok(dataResponseForSvedenOOPDGUs);
        }
    }
}
