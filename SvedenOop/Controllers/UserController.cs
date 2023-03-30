using DomainServices.Entities;
using DomainServices.DtoModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ifrastructure.Repository.InterfaceRepository;
using DomainServices.DtoModels;
using BasePersonDBService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EorDSU.Controllers
{
    [Authorize(Roles = "admin")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IUmuAndFacultyRepository _umuAndFacultyRepository;
        private readonly IBasePersonActiveData _basePersonActiveData;

        public UserController(UserManager<User> userManager, IUmuAndFacultyRepository umuAndFacultyRepository, IBasePersonActiveData basePersonActiveData)
        {
            _userManager = userManager;
            _umuAndFacultyRepository = umuAndFacultyRepository;
            _basePersonActiveData = basePersonActiveData;
        }

        [Route("GetUsers")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            List<FullInfoUserDto> dtoUsers = new();
            foreach (var item in _userManager.Users.ToList())
            {
                var roles = await _userManager.GetRolesAsync(item);
                dtoUsers.Add(new FullInfoUserDto()
                {
                    User = item,
                    Department = _basePersonActiveData.GetPersDepartmentById(item.PersDepartmentId),
                    Faculties = _umuAndFacultyRepository.Get().Where(x => x.UserId == item.Id)
                                                              .Select(c => _basePersonActiveData.GetPersDivisionById(c.FacultyId)).ToList(),
                    Role = roles.First()
                });
            }
            return Ok(dtoUsers);
        }

        [Route("GetUserById")]
        [HttpGet]
        public async Task<IActionResult> GetUserById(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return BadRequest("Такой пользователь не найден");
            var roles = await _userManager.GetRolesAsync(user);
            FullInfoUserDto model = new()
            {
                User = user,
                Department = _basePersonActiveData.GetPersDepartmentById(user.PersDepartmentId),
                Faculties = _umuAndFacultyRepository.Get().Where(x => x.UserId == user.Id)
                                                              .Select(c => _basePersonActiveData.GetPersDivisionById(c.FacultyId)).ToList(),
                Role = roles.First()
            };
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
                    await _umuAndFacultyRepository.CreateRange(faculties);
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
                    await _umuAndFacultyRepository.CreateRange(faculties);
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
            User user = _userManager.Users.Include(x => x.Faculty).FirstOrDefault(c => c.Id == id);
            if (user == null)
                return BadRequest("Пользователь не найден");

            await _userManager.DeleteAsync(user);
            return Ok();
        }
    }
}
