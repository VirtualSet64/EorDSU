using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SvedenOop.Services.Interfaces;

namespace SvedenOop.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class JsonFileGenerationController : Controller
    {
        private readonly IGenerateJsonService _generateJsonService;

        public JsonFileGenerationController(IGenerateJsonService generateJsonService)
        {
            _generateJsonService = generateJsonService;
        }

        [HttpGet]
        [Route("GenerateJsonFile")]
        public IActionResult GenerateJsonFile()
        {
            _generateJsonService.GenerateJsonFileForOopDgu();
            _generateJsonService.GenerateJsonFileForOpop2();
            return Ok();
        }
    }
}
