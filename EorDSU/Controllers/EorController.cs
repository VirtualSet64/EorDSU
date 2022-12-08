using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;
using EorDSU.Repository;
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
        private readonly DSUContext _dSUContext;
        private readonly IActiveData _activeData;
        private readonly IConfiguration Configuration;

        public EorController(IActiveData activeData, IConfiguration configuration, DSUContext dSUContext)
        {
            _activeData = activeData;
            Configuration = configuration;
            _dSUContext = dSUContext;            
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

            var persDepartments = _activeData.GetPersDepartments().Where(x => x.DivId == facultyId).ToList();

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
                CaseCEdukind = _dSUContext.CaseCEdukinds.FirstOrDefault(x => x.EdukindId == item.CaseCEdukindId),
                CaseSDepartment = _dSUContext.CaseSDepartments.FirstOrDefault(x => x.DepartmentId == item.CaseSDepartmentId),
                SrokDeystvGosAccred = Configuration["SrokDeystvGosAccred"],
            });
        }
    }
}
