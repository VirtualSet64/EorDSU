using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;
using EorDSU.Repository;
using EorDSU.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private readonly IActiveData _activeData;
        private readonly ApplicationContext _applicationContext;

        public AdminController(IActiveData activeData, ApplicationContext applicationContext)
        {
            _activeData = activeData;
            _applicationContext = applicationContext;
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
    }
}
