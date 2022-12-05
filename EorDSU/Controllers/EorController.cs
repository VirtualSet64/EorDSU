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

        public EorController(IActiveData activeData, DSUContext dSUContext, IConfiguration configuration)
        {
            _activeData = activeData;
            _dSUContext = dSUContext;
            Configuration = configuration;
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
                dataResponseForSvedenOOPDGUs.Add(new()
                {
                    Profiles = item,
                    CaseCEdukind = _dSUContext.CaseCEdukinds.FirstOrDefault(x => x.EdukindId == item.CaseCEdukindId),                
                    CaseSDepartment = _dSUContext.CaseSDepartments.FirstOrDefault(x => x.DepartmentId == item.CaseSDepartmentId),                
                    SrokDeystvGosAccred = Configuration["SrokDeystvGosAccred"],                
                });
            }
            return Ok(dataResponseForSvedenOOPDGUs);
        }
    }
}
