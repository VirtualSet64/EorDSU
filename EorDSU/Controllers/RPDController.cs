using EorDSU.ConfigInfo;
using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace EorDSU.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RPDController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IConfiguration Configuration;

        public RPDController(ApplicationContext context, IWebHostEnvironment appEnvironment, IConfiguration configuration)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetDisciplines(int profileId)
        {
            return Ok(_context.Disciplines.Where(x => x.ProfileId == profileId).ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disciplineId"></param>
        /// <param name="uploadedFile"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddRPD(int disciplineId, IFormFile uploadedFile, int year)
        {
            Discipline discipline = _context.Disciplines.FirstOrDefault(x => x.Id == disciplineId);

            string path = Configuration["FileFolder"] + uploadedFile.FileName;
            // сохраняем файл в папку Files в каталоге wwwroot
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                uploadedFile.CopyToAsync(fileStream);
            }
            
            FileRPD file = new() { Name = uploadedFile.FileName, Year = year };
            discipline.FileRPD = file;
            _context.FileRPDs.Add(file);
            _context.Disciplines.Update(discipline);
            _context.SaveChanges();
            return Ok();
        }
    }
}
