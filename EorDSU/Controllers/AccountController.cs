using DomainServices.Models;
using DomainServices.DtoModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EorDSU.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
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
                        var userIncludeRoles = new UserIncludeRolesViewModel
                        {
                            UserId = user.Id,
                            UserName = user?.UserName,
                            PersDepartmentId = user.PersDepartmentId,
                        };
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles != null && roles.Any())
                            userIncludeRoles.Role = roles.First();

                        return Ok(userIncludeRoles);
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
