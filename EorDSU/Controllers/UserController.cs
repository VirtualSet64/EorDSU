using DomainServices.Entities;
using DomainServices.DtoModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EorDSU.Controllers
{
    [Authorize(Roles = "admin")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [Route("GetUsers")]
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_userManager.Users.ToList());
        }

        [Route("GetUser")]
        [HttpGet]
        public async Task<IActionResult> GetUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            EditViewModel model = new() { Id = user.Id, Login = user.UserName };
            return Ok(model);
        }

        [Route("CreateUser")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new() { UserName = model.Login, PersDepartmentId = model.PersDepartmentId };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (model.Role != null)
                {
                    List<string> roles = new() { model.Role };
                    await _userManager.AddToRolesAsync(user, roles);
                }
                if (model.Faculties != null && model.Faculties.Count != 0)
                {
                    var faculties = new List<UmuAndFaculty>();
                    foreach (var item in model.Faculties)
                    {
                        faculties.Add(new UmuAndFaculty()
                        {
                            FacultyId = item,
                            UserId = user.Id,
                        });
                    }
                    await _userManager.UpdateAsync(user);
                }
                if (result.Succeeded)
                    return Ok();
            }
            return BadRequest();
        }

        [Route("EditUser")]
        [HttpPut]
        public async Task<IActionResult> EditUser(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                var _passwordHasher = HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;
                if (user != null)
                {
                    user.UserName = model.Login;
                    user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
                    if (model.PersDepartmentId != null)
                        user.PersDepartmentId = (int)model.PersDepartmentId;

                    if (model.Role != null)
                    {
                        List<string> roles = new() { model.Role };
                        // получем список ролей пользователя
                        var userRoles = await _userManager.GetRolesAsync(user);
                        // получаем список ролей, которые были добавлены
                        var addedRoles = roles.Except(userRoles);
                        // получаем роли, которые были удалены
                        var removedRoles = userRoles.Except(roles);

                        await _userManager.AddToRolesAsync(user, addedRoles);

                        await _userManager.RemoveFromRolesAsync(user, removedRoles);

                        return Ok();
                    }
                    var faculties = new List<UmuAndFaculty>();
                    foreach (var item in model.Faculties)
                    {
                        faculties.Add(new UmuAndFaculty()
                        {
                            FacultyId = item,
                            UserId = user.Id,
                        });
                    }
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return Ok();
                }
            }
            return BadRequest();
        }

        [Route("DeleteUser")]
        [HttpDelete]
        public async Task<ActionResult> DeleteUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
                await _userManager.DeleteAsync(user);
            return Ok();
        }
    }
}
