using EorDSU.DBService;
using EorDSU.Models;
using EorDSU.ResponseModel;
using EorDSU.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EorDSU.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FilesController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IConfiguration Configuration;
        private readonly ExcelParsingService _excelParsingService;

        public FilesController(ApplicationContext context, IWebHostEnvironment appEnvironment, IConfiguration configuration, ExcelParsingService excelParsingService)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            Configuration = configuration;
            _excelParsingService = excelParsingService;
        }

        /// <summary>
        /// Добавление файлов
        /// </summary>
        /// <param name="uploadedFile">Файл</param>
        /// <param name="fileTypeId">Тип Файла</param>
        /// <param name="profileId">Профиль</param>
        /// <param name="year"></param>
        /// <returns></returns>
        [Route("AddFile")]
        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile, int fileTypeId = 2, int profileId = 0)
        {
            if (uploadedFile == null)
                return BadRequest();

            // путь к папке Files
            string path = Configuration["FileFolder"] + "/" + uploadedFile.FileName;
            // сохраняем файл в папку Files в каталоге wwwroot
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(fileStream);
            }

            if (fileTypeId == (int)FileType.EduPlan && (Path.GetExtension(path) == ".xls" || Path.GetExtension(path) == ".xlsx"))
                return Ok(_excelParsingService.ParsingService(path));

            FileModel file = new() { Name = uploadedFile.FileName, ProfileId = profileId, Type = (FileType)fileTypeId };

            _context.FileModels.Add(file);
            _context.SaveChanges();

            return Ok(file);
        }

        /// <summary>
        /// Изменение файла
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="fileId"></param>
        /// <param name="profileId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [Route("EditFile")]
        [HttpPut]
        public async Task<IActionResult> EditFile(IFormFile uploadedFile, int fileId, int profileId = 0)
        {
            if (uploadedFile == null && profileId <= 0)
                return BadRequest();

            var filedb = _context.FileModels.FirstOrDefault(c => c.Id == fileId);
            // путь к папке Files
            string path = Configuration["FileFolder"] + uploadedFile.FileName;
            // сохраняем файл в папку Files в каталоге wwwroot
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(fileStream);
            }

            FileModel file = new() { Name = uploadedFile.FileName, ProfileId = profileId };
            _context.FileModels.Update(file);
            _context.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [Route("DeleteFile")]
        [HttpDelete]
        public IActionResult DeleteFile(int fileId)
        {
            FileModel? file = _context.FileModels.FirstOrDefault(x => x.Id == fileId);

            if (file == null)
                return BadRequest();

            _context.FileModels.Remove(file);
            _context.SaveChanges();
            return Ok();
        }
    }
}
