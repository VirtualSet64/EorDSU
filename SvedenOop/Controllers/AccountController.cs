using DomainServices.Entities;
using DomainServices.DtoModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DomainServices.DtoModels;
using BasePersonDBService.Interfaces;
using Ifrastructure.Repository.InterfaceRepository;

namespace SvedenOop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IBasePersonActiveData _basePersonActiveData;
        private readonly IUmuAndFacultyRepository _umuAndFacultyRepository;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, IBasePersonActiveData basePersonActiveData, IUmuAndFacultyRepository umuAndFacultyRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _basePersonActiveData = basePersonActiveData;
            _umuAndFacultyRepository = umuAndFacultyRepository;
        }

        [Route("Logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, false, false);
                if (result.Succeeded)
                {
                    var user = _userManager.Users.FirstOrDefault(x => x.UserName == model.Login);
                    if (user != null)
                    {
                        var fullInfoUserDto = new FullInfoUserDto
                        {
                            User = user,
                            Department = _basePersonActiveData.GetPersDepartmentById(user.PersDepartmentId),
                            Faculties = _umuAndFacultyRepository.Get().Where(x => x.UserId == user.Id)
                                                              .Select(c => _basePersonActiveData.GetPersDivisionById(c.FacultyId)).ToList()
                        };
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles != null && roles.Any())
                            fullInfoUserDto.Role = roles.First();

                        return Ok(fullInfoUserDto);
                    }
                    else
                        return BadRequest("Пользователь с таким логином не найден");
                }
                else
                    return BadRequest("Неправильный логин и (или) пароль");
            }
            return BadRequest();
        }
    }
}
