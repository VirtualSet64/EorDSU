using EorDSU.ConfigInfo;
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
        private readonly ApplicationContext _context;
        private readonly DSUContext _dSUContext;
        private readonly IActiveData _activeData;
        private readonly AuthOptions _authOptions;
        private readonly IConfiguration Configuration;

        public EorController(ApplicationContext context, AuthOptions authOptions, IActiveData activeData, DSUContext dSUContext, IConfiguration configuration)
        {
            _context = context;
            _authOptions = authOptions;
            _activeData = activeData;
            _dSUContext = dSUContext;
            Configuration = configuration;
        }

        /// <summary>
        /// Получение всеъ данных 
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

        /// <summary>
        /// Вход
        /// </summary>
        /// <param name="requestUser"></param>
        /// <returns></returns>
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(User requestUser)
        {
            User? user = _context.Users.FirstOrDefault(p => p.UserName == requestUser.UserName && p.Password == requestUser.Password);
            // если пользователь не найден, отправляем статусный код 401
            if (user is null) return BadRequest();

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.UserName) };
            // создаем JWT-токен
            var jwt = _authOptions.GetAuthData(claims);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            // формируем ответ
            var response = new
            {
                access_token = encodedJwt,
                username = user.UserName
            };

            var profiles = await _activeData.GetProfiles().Where(x => x.PersDepartmentId == user.PersDepartmentId).ToListAsync();
            List<DataResponseForSvedenOOPDGU> dataResponseForSvedenOOPDGUs = new();
            foreach (var item in profiles)
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
