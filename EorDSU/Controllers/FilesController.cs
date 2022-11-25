﻿using EorDSU.DBService;
using EorDSU.Models;
using EorDSU.ResponseModel;
using EorDSU.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EorDSU.Controllers
{
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
        /// 
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="fileTypeId"></param>
        /// <param name="profileId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<List<Profile>>> AddFile(IFormFile uploadedFile, int fileTypeId, int year = 0)
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

                if (fileTypeId == (int)FileType.EduPlan && (Path.GetExtension(path) == ".xls" || Path.GetExtension(path) == ".xlsx"))
                {
                    return Ok(_excelParsingService.ParsingService(path));
                }
                //else
                //{
                //    FileModel file = new() { Name = uploadedFile.FileName, ProfileId = profileId, Year = year, Type = (FileType)fileTypeId };
                //    _context.FileModels.Add(file);
                //    _context.SaveChanges();
                //    return Ok(file);
                //}
            }
            return BadRequest();
        }
                
        /// <summary>
        /// Изменение файла
        /// </summary>
        /// <param name="uploadedFile">Обьект файла</param>
        /// <param name="fileType">Тип файла</param>
        /// <param name="year">Год обучения</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> EditFile(IFormFile uploadedFile, int fileId, int profileId = -1, int year = 0)
        {
            if (uploadedFile != null)
            {
                if (profileId > 0)
                {
                    var filedb = _context.FileModels.FirstOrDefault(c => c.Id == fileId);
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

                    FileModel file = new() { Name = uploadedFile.FileName, ProfileId = profileId, Year = year };
                    _context.FileModels.Update(file);
                    _context.SaveChanges();
                    return Ok();
                }
            }
            return BadRequest();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteFile(int fileId)
        {
            FileModel file = _context.FileModels.FirstOrDefault(x => x.Id == fileId);

            if (file != null)
            {
                _context.FileModels.Remove(file);
                _context.SaveChanges();
                return Ok();
            }
            else
                return BadRequest();
        }
    }
}
