using BasePersonDBService.Interfaces;
using DSUContextDBService.Interfaces;
using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;
using EorDSU.ResponseModel;
using EorDSU.Service;
using EorDSU.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;

namespace EorDSU.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EorController : Controller
    {
        private readonly IApplicationActiveData _activeData;
        private readonly IBasePersonActiveData _basePersonActiveData;
        private readonly IDSUActiveData _dSUActiveData;
        private readonly IConfiguration Configuration;

        public EorController(IApplicationActiveData activeData, IConfiguration configuration, IBasePersonActiveData basePersonActiveData, IDSUActiveData dSUActiveData)
        {
            _activeData = activeData;
            Configuration = configuration;       
            _basePersonActiveData = basePersonActiveData;
            _dSUActiveData = dSUActiveData;
        }

        /// <summary>
        /// Получение всех данных 
        /// </summary>
        /// <returns></returns>
        [Route("GetData")]
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            List<DataResponseForSvedenOOPDGU> dataResponseForSvedenOOPDGUs = new();
            foreach (var item in await _activeData.GetProfiles().ToListAsync())
            {
                FillingData(dataResponseForSvedenOOPDGUs, item);
            }
            return Ok(dataResponseForSvedenOOPDGUs);
        }

        /// <summary>
        /// Получение всех данных 
        /// </summary>
        /// <returns></returns>
        [Route("GetDataById")]
        [HttpGet]
        public async Task<IActionResult> GetData(int cafedraId)
        {
            List<DataResponseForSvedenOOPDGU> dataResponseForSvedenOOPDGUs = new();

            foreach (var item in await _activeData.GetProfiles().Where(x => x.PersDepartmentId == cafedraId).ToListAsync())
            {
                FillingData(dataResponseForSvedenOOPDGUs, item);
            }
            return Ok(dataResponseForSvedenOOPDGUs);
        }

        /// <summary>
        /// Получение всех данных
        /// </summary>
        /// <returns></returns>
        [Route("GetDataFacultyById")]
        [HttpGet]
        public async Task<IActionResult> GetDataFacultyById(int facultyId)
        {
            List<DataResponseForSvedenOOPDGU> dataResponseForSvedenOOPDGUs = new();

            var persDepartments = _basePersonActiveData.GetPersDepartments().Where(x => x.DivId == facultyId).ToList();

            foreach (var persDepartment in persDepartments)
            {
                foreach (var item in await _activeData.GetProfiles().Where(x => x.PersDepartmentId == persDepartment.DepId).ToListAsync())
                {
                    FillingData(dataResponseForSvedenOOPDGUs, item);
                }
            }
            return Ok(dataResponseForSvedenOOPDGUs);
        }

        private void FillingData(List<DataResponseForSvedenOOPDGU> dataResponseForSvedenOOPDGUs, Profile item)
        {
            dataResponseForSvedenOOPDGUs.Add(new()
            {
                Profiles = item,
                CaseCEdukind = _dSUActiveData.GetCaseCEdukind((int)item.CaseCEdukindId),
                CaseSDepartment = _dSUActiveData.GetCaseSDepartment((int)item.CaseSDepartmentId),
                SrokDeystvGosAccred = Configuration["SrokDeystvGosAccred"],
            });
        }
    }
}
