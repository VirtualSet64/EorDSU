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
        private readonly IActiveData _activeData;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IConfiguration Configuration;

        public RPDController(ApplicationContext context, IWebHostEnvironment appEnvironment, IConfiguration configuration, IActiveData activeData)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            Configuration = configuration;
            _activeData = activeData;
        }

        /// <summary>
        /// Получение всех дисциплин данного профиля
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        [Route("GetDisciplines")]
        [HttpGet]
        public IActionResult GetDisciplines(int profileId)
        {
            return Ok(_activeData.GetDisciplines().Where(x => x.ProfileId == profileId).ToList());
        }

        /// <summary>
        /// Добавление РПД к дисциплине
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="disciplineId"></param>
        /// <returns></returns>
        [Route("AddRPD")]
        [HttpPost]
        public IActionResult AddRPD(IFormFile uploadedFile, int disciplineId)
        {
            Discipline? discipline = _activeData.GetDisciplines().FirstOrDefault(x => x.Id == disciplineId);

            if (discipline == null || uploadedFile == null)
                return BadRequest();

            string path = Configuration["FileFolder"] + uploadedFile.FileName;
            // сохраняем файл в папку Files в каталоге wwwroot
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                uploadedFile.CopyToAsync(fileStream);
            }            

            FileRPD file = new() { Name = uploadedFile.FileName };
            discipline.FileRPD = file;
            _context.FileRPDs.Add(file);
            _context.Disciplines.Update(discipline);
            _context.SaveChanges();
            return Ok();
        }
    }
}
