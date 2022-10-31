using EorDSU.ConfigInfo;
using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;
using EorDSU.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EorDSU.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EorController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly BASEPERSONMDFContext _basePersonContext;
        private readonly AuthOptions _authOptions;

        public EorController(ApplicationContext context, BASEPERSONMDFContext basePersonContext, AuthOptions authOptions)
        {
            _context = context;
            _basePersonContext = basePersonContext;
            _authOptions = authOptions;
        }

        [HttpGet]
        public List<FileModel> GetFiles()
        {
            return _context.FileModels.ToList();
        }

        /// <summary>
        /// Вход
        /// </summary>
        /// <param name="requestUser"></param>
        /// <returns></returns>
        [HttpPost]
        public IResult Login(User requestUser)
        {
            User? user = _context.Users.FirstOrDefault(p => p.UserName == requestUser.UserName && p.Password == requestUser.Password);
            // если пользователь не найден, отправляем статусный код 401
            if (user is null) return Results.Unauthorized();

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

            return Results.Json(response);
        }
    }
}
