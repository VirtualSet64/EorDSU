using EorDSU.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EorDSU.Controllers
{
    [Authorize(Roles = "admin")]
    [ApiController]
    [Route("[controller]")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [Route("GetRoles")]
        [HttpGet]
        public IActionResult GetRoles()
        {
            return Ok(_roleManager.Roles.ToList());
        }

        [Route("CreateRole")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                    return Ok();
            }
            return BadRequest("Некорректное имя роли");
        }

        [Route("EditRole")]
        [HttpPut]
        public async Task<IActionResult> EditRole(string userId, string role)
        {
            // получаем пользователя
            User user = await _userManager.FindByIdAsync(userId);
            List<string> roles = new() { role };
            if (user != null)
            {
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
            return NotFound("Такой пользователь не найден");
        }

        [Route("DeleteRole")]
        [HttpDelete]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return BadRequest("Роль не найдена");

            await _roleManager.DeleteAsync(role);
            return Ok();
        }
    }
}
