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
        private readonly AuthOptions _authOptions;

        public EorController(ApplicationContext context, AuthOptions authOptions)
        {
            _context = context;
            _authOptions = authOptions;
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            DataResponseForSvedenOOPDGU dataResponseForSvedenOOPDGU = new()
            {
                Profiles = await _context.Profiles
                        .Include(x => x.LevelEdu)
                        .Include(x => x.CaseSDepartment)
                        .Include(x => x.PersDepartment)
                        .ToListAsync(),
                SrokDeystvGosAccred = "24.04.2025",
            };
            //dataResponseForAll.PersDivision = _activeData.GetPersDivisions();
            //dataResponseForAll.CaseSDepartments = await _DSUContextContext.CaseSDepartments.ToListAsync();
            //dataResponseForAll.CaseCEdukinds = await _DSUContextContext.CaseCEdukinds.ToListAsync();
            //dataResponseForAll.LevelEdus = await _context.LevelEdues.ToListAsync();
            //dataResponseForAll.Profiles = await _context.Profiles.ToListAsync();
            //dataResponseForAll.FileModels = await _context.FileModels.ToListAsync();
            //dataResponseForAll.SrokDeystvGosAccred = "24.04.2025";

            return Ok(dataResponseForSvedenOOPDGU);
        }

        /// <summary>
        /// Вход
        /// </summary>
        /// <param name="requestUser"></param>
        /// <returns></returns>
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

            ////// Переделать
            DataResponseForSvedenOOPDGU dataResponseForSvedenOOPDGU = new()
            {
                Profiles = await _context.Profiles
                            .Include(x => x.LevelEdu)
                            .Include(x => x.CaseSDepartment)
                            .Include(x => x.PersDepartment)
                            .ToListAsync(),
                SrokDeystvGosAccred = "24.04.2025",
            };
            ///////
            return Ok(response);
        }
    }
}
