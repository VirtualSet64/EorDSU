using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EorDSU.Controllers
{
    [Authorize(Roles = "admin")]
    [ApiController]
    [Route("[controller]")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
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
