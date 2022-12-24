using EorDSU.DBService;
using EorDSU.Interface;
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
        private readonly IApplicationActiveData _activeData;

        public FilesController(ApplicationContext context, IWebHostEnvironment appEnvironment, IConfiguration configuration, ExcelParsingService excelParsingService, IApplicationActiveData activeData)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            Configuration = configuration;
            _excelParsingService = excelParsingService;
            _activeData = activeData;
        }
        
        /// <summary>
        /// Добавление РПД к дисциплине
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="disciplineId"></param>
        /// <returns></returns>
        [Route("AddRPD")]
        [HttpPost]
        public async Task<IActionResult> AddRPD(IFormFile uploadedFile, int disciplineId)
        {
            Discipline? discipline = _activeData.GetDisciplines().FirstOrDefault(x => x.Id == disciplineId);

            if (discipline == null || uploadedFile == null)
                return BadRequest();

            string path = Configuration["FileFolder"] + uploadedFile.FileName;
            // сохраняем файл в папку Files в каталоге wwwroot
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(fileStream);
            }

            FileRPD file = new() { Name = uploadedFile.FileName };
            discipline.FileRPD = file;
            _context.FileRPDs.Add(file);
            _context.Disciplines.Update(discipline);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Изменение файла РПД
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="fileId"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        [Route("EditRPD")]
        [HttpPut]
        public async Task<IActionResult> EditRPD(IFormFile uploadedFile, int fileRPDId, int disciplineId)
        {
            if (uploadedFile == null && disciplineId == 0)
                return BadRequest();

            var fileRPDdb = _context.FileRPDs.FirstOrDefault(c => c.Id == fileRPDId);
            // путь к папке Files
            string path = Configuration["FileFolder"] + uploadedFile.FileName;
            // сохраняем файл в папку Files в каталоге wwwroot
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(fileStream);
            }

            FileRPD fileRPD = new() { Name = uploadedFile.FileName, DisciplineId = disciplineId };
            _context.FileRPDs.Update(fileRPD);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Удаление файла РПД
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [Route("DeleteRPD")]
        [HttpDelete]
        public async Task<IActionResult> DeleteRPD(int fileRPDId)
        {
            var rpd = _context.FileRPDs.FirstOrDefault(x => x.Id == fileRPDId);

            if (rpd == null)
                return BadRequest();

            _context.FileRPDs.Remove(rpd);
            await _context.SaveChangesAsync();
            return Ok();
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
                return Ok(await _excelParsingService.ParsingService(path));

            FileModel file = new() { Name = uploadedFile.FileName, ProfileId = profileId, Type = (FileType)fileTypeId };

            _context.FileModels.Add(file);
            await _context.SaveChangesAsync();

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
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Удаление файла
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [Route("DeleteFile")]
        [HttpDelete]
        public async Task<IActionResult> DeleteFile(int fileId)
        {
            FileModel? file = _context.FileModels.FirstOrDefault(x => x.Id == fileId);

            if (file == null)
                return BadRequest();

            _context.FileModels.Remove(file);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
