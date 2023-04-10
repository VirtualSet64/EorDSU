using Ifrastructure.Repository.InterfaceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SvedenOop.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LevelEduController : Controller
    {
        private readonly ILevelEduRepository _levelEduRepository;

        public LevelEduController(ILevelEduRepository levelEduRepository)
        {
            _levelEduRepository = levelEduRepository;
        }

        [Route("GetLevelEdu")]
        [HttpGet]
        public IActionResult GetLevelEdu()
        {
            return Ok(_levelEduRepository.Get());
        }
    }
}
