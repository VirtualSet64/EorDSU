using DSUContextDBService.Interfaces;
using EorDSU.Common.Interfaces;
using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;
using EorDSU.Repository.InterfaceRepository;
using EorDSU.ResponseModel;
using EorDSU.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sentry;

namespace EorDSU.Controllers
{
    //[Authorize(Roles = "admin")]
    [ApiController]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration Configuration;

        public AdminController(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("GetFileModels")]
        [HttpGet]
        public async Task<List<FileModel>> GetFileModelsAsync()
        {
            return await _unitOfWork.FileModelRepository.Get().ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("GetFileRPDs")]
        [HttpGet]
        public async Task<List<FileRPD>> GetFileRPDsAsync()
        {
            return await _unitOfWork.FileRPDRepository.Get().ToListAsync();
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="persDepartmentId"></param>
        ///// <returns></returns>
        //[Route("GetDataAsync")]
        //[HttpGet]
        //public async Task<IActionResult> GetDataAsync(int persDepartmentId)
        //{
        //    var profiles = await _activeData.GetProfiles().Where(x => x.PersDepartmentId == persDepartmentId).ToListAsync();
        //    if (profiles == null || profiles.Count == 0)
        //        return BadRequest();

        //    List<DataResponseForSvedenOOPDGU> dataResponseForSvedenOOPDGUs = new();
        //    foreach (var item in profiles)
        //    {
        //        dataResponseForSvedenOOPDGUs.Add(new()
        //        {
        //            Profiles = item,
        //            CaseCEdukind = _dSUActiveData.GetCaseCEdukindById((int)item.CaseCEdukindId),
        //            CaseSDepartment = _dSUActiveData.GetCaseSDepartmentById((int)item.CaseSDepartmentId),
        //            SrokDeystvGosAccred = Configuration["SrokDeystvGosAccred"],
        //        });
        //    }
        //    return Ok(dataResponseForSvedenOOPDGUs);
        //}
    }
}