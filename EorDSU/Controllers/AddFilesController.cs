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
    public class AddFilesController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IConfiguration Configuration;
        private readonly ExcelParsingService _excelParsingService;

        public AddFilesController(ApplicationContext context, IWebHostEnvironment appEnvironment, IConfiguration configuration, ExcelParsingService excelParsingService)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            Configuration = configuration;
            _excelParsingService = excelParsingService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uploadedFile">Обьект файла</param>
        /// <param name="fileType">Тип файла</param>
        /// <param name="year">Год обучения</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile, int fileTypeId, int profileId = -1, int year = 0)
        {
            if (uploadedFile != null)
            {
                if (year == 0)
                {
                    year = DateTime.Now.Year;
                }

                // путь к папке Files
                string path = Configuration["FileFolder"] + "/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                if (Path.GetExtension(path) == ".xls" || Path.GetExtension(path) == ".xlsx")
                {
                    if (fileTypeId == (int)FileType.EduPlan)
                    {
                        _excelParsingService.ParsingService(path);
                    }
                    else
                    {
                        FileModel file = new() { Name = uploadedFile.FileName, Profile = _context.Profiles.FirstOrDefault(x => x.Id == profileId), Year = year, Type = (FileType)fileTypeId };
                        _context.FileModels.Add(file);
                        _context.SaveChanges();
                        return Ok(file);
                    }
                }
            }
            return Ok();
        }

        /// <summary>
        /// Изменение файла
        /// </summary>
        /// <param name="uploadedFile">Обьект файла</param>
        /// <param name="fileType">Тип файла</param>
        /// <param name="year">Год обучения</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<object> EditFile(IFormFile uploadedFile, int id, int fileType, int year = 0)
        {
            if (uploadedFile != null)
            {
                var filedb = _context.FileModels.FirstOrDefault(c => c.Id == id);
                if (filedb != null && year == 0)
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

                FileModel file = new() { Name = uploadedFile.FileName, Year = year };
                _context.FileModels.Update(file);
                _context.SaveChanges();
                return file;
            }
            return Ok();
        }
    }
}
