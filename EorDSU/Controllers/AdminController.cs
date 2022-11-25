using EorDSU.DBService;
using EorDSU.Models;
using EorDSU.Service;
using Microsoft.AspNetCore.Mvc;

namespace EorDSU.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private readonly ApplicationContext _context;

        public AdminController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<FileModel> GetFileModels()
        {
            return _context.FileModels.ToList();
        }

        [Route("GetFileRPDs")]
        [HttpGet]
        public List<FileRPD> GetFileRPDs()
        {
            return _context.FileRPDs.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddFinalFormFile(Profile profile)
        {
            if (profile != null && profile != null && profile.LevelEdu != null && profile.Disciplines != null)
            {
                _context.LevelEdues.Add(profile.LevelEdu);
                _context.Profiles.Add(profile);
                _context.Disciplines.AddRange(profile.Disciplines);
                _context.SaveChanges();
                return Ok();
            }
            else
                return BadRequest();
        }
    }
}
