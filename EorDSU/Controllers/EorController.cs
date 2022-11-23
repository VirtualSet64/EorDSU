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
        private readonly DSUContext _DSUContextContext;
        private readonly AuthOptions _authOptions;
        private readonly IActiveData _activeData;

        public EorController(ApplicationContext context, DSUContext DSUContextContext, AuthOptions authOptions, IActiveData activeData)
        {
            _context = context;
            _authOptions = authOptions;
            _DSUContextContext = DSUContextContext;
            _activeData = activeData;
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            DataResponseForAll dataResponseForAll = new()
            {
                PersDivision = _activeData.GetPersDivisions(),
                CaseSDepartments = await _DSUContextContext.CaseSDepartments.ToListAsync(),
                CaseCEdukinds = await _DSUContextContext.CaseCEdukinds.ToListAsync(),
                LevelEdus = await _context.LevelEdues.ToListAsync(),
                Profiles = await _context.Profiles.ToListAsync(),
                FileModels = await _context.FileModels.ToListAsync(),
                SrokDeystvGosAccred = "24.04.2025",
            };
            //dataResponseForAll.PersDivision = _activeData.GetPersDivisions();
            //dataResponseForAll.CaseSDepartments = await _DSUContextContext.CaseSDepartments.ToListAsync();
            //dataResponseForAll.CaseCEdukinds = await _DSUContextContext.CaseCEdukinds.ToListAsync();
            //dataResponseForAll.LevelEdus = await _context.LevelEdues.ToListAsync();
            //dataResponseForAll.Profiles = await _context.Profiles.ToListAsync();
            //dataResponseForAll.FileModels = await _context.FileModels.ToListAsync();
            //dataResponseForAll.SrokDeystvGosAccred = "24.04.2025";
            foreach (var item in dataResponseForAll.Profiles)
            {
                dataResponseForAll.Years.Add((int)item.Year);
            }

            return Ok(dataResponseForAll);
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
            PersDivision faculty = _activeData.GetPersDivisions().FirstOrDefault(x => x.DivId == requestUser.PersDepartment.DivId);
            DataResponseForMethodist dataResponseForMethodist = new()
            {
                CaseSDepartments = await _DSUContextContext.CaseSDepartments.Where(x => x.FacId == faculty.DivId).ToListAsync(),
                CaseCEdukinds = await _DSUContextContext.CaseCEdukinds.ToListAsync(),
                LevelEdus = await _context.LevelEdues.ToListAsync(),
                Profiles = await _context.Profiles.ToListAsync(),
                FileModels = await _context.FileModels.ToListAsync(),
                SrokDeystvGosAccred = "24.04.2025",
            };
            ///////
            return Ok(response);
        }
    }
}
