using EorDSU.ConfigInfo;
using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;
using EorDSU.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EorDSU.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AddFilesController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ISearchEntity _searchEntity;
        private readonly ParsingService _parsingService;
        private readonly IConfiguration Configuration;

        public AddFilesController(ApplicationContext context, IWebHostEnvironment appEnvironment, ISearchEntity searchEntity, ParsingService parsingService, IConfiguration configuration)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _searchEntity = searchEntity;
            _parsingService = parsingService;
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<FileModel> GetFiles()
        {
            return _context.FileModels.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uploadedFile">Обьект файла</param>
        /// <param name="fileType">Тип файла</param>
        /// <param name="profile">Профиль</param>
        /// <param name="year">Год обучения</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> AddFile(IFormFile uploadedFile, int fileType, string profile, int year = 0)
        {
            if (uploadedFile != null)
            {
                if (year == 0)
                {
                    year = DateTime.Now.Year;
                }
                // путь к папке Files
                string path = Configuration["FileFolder"] + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                FileModel file = new() { Name = uploadedFile.FileName, Year = year, FileType = fileType, Profile = _searchEntity.SearchProfile(profile) };
                _context.FileModels.Add(file);
                _context.SaveChanges();
                if (fileType == 5)//если рпд под пятым номером
                {
                    return _parsingService.ParsingRPD(path);
                }
                return file;
            }
            return Ok();
        }

        /// <summary>
        /// Изменение файла
        /// </summary>
        /// <param name="uploadedFile">Обьект файла</param>
        /// <param name="fileType">Тип файла</param>
        /// <param name="profile">Профиль</param>
        /// <param name="year">Год обучения</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<object> EditFile(IFormFile uploadedFile, int id, int fileType, string profile, int year = 0)
        {
            if (uploadedFile != null)
            {
                var filedb = _context.FileModels.FirstOrDefault(c => c.Id == id);
                if (filedb != null && filedb.Year == 0)
                {
                    filedb.Year = DateTime.Now.Year;
                }
                // путь к папке Files
                string path = Configuration["FileFolder"] + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                FileModel file = new() { Name = uploadedFile.FileName, Year = year, FileType = fileType, Profile = _searchEntity.SearchProfile(profile) };
                _context.FileModels.Update(file);
                _context.SaveChanges();
                if (fileType == 5)//если рпд под пятым номером
                {
                    return _parsingService.ParsingRPD(path);
                }
                return file;
            }
            return Ok();
        }
    }
}
