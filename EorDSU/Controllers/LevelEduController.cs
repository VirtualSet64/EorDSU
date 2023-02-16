using EorDSU.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EorDSU.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LevelEduController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public LevelEduController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Route("GetLevelEdu")]
        [HttpGet]
        public IActionResult GetLevelEdu()
        {
            return Ok(_unitOfWork.LevelEduRepository.Get());
        }
    }
}
