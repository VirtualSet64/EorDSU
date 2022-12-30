using EorDSU.Models;
using EorDSU.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sentry;

namespace EorDSU.Controllers
{
    [Authorize(Roles = "admin")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserManager<Models.User> _userManager;

        public UserController(UserManager<Models.User> userManager)
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
            try
            {
                Models.User user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return NotFound();

                EditViewModel model = new() { Id = user.Id, Login = user.UserName };
                return Ok(model);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw;
            }
        }

        [Route("CreateUser")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Models.User user = new() { UserName = model.Login, PersDepartmentId = model.PersDepartmentId };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        return Ok();
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw;
            }
        }

        [Route("EditUser")]
        [HttpPut]
        public async Task<IActionResult> EditUser(EditViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Models.User user = await _userManager.FindByIdAsync(model.Id);
                    if (user != null)
                    {
                        user.UserName = model.Login;
                        user.PersDepartmentId = model.PersDepartmentId;

                        var result = await _userManager.UpdateAsync(user);
                        if (result.Succeeded)
                            return Ok();
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw;
            }
        }

        [Route("DeleteUser")]
        [HttpDelete]
        public async Task<ActionResult> DeleteUser(string id)
        {
            try
            {
                Models.User user = await _userManager.FindByIdAsync(id);
                if (user != null)
                    await _userManager.DeleteAsync(user);
                return Ok();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw;
            }
        }
    }
}
