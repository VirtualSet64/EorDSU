using BasePersonDBService.Interfaces;
using BasePersonDBService.Services;
using DSUContextDBService.Interfaces;
using DSUContextDBService.Services;
using EorDSU.Common.Interfaces;
using EorDSU.Interface;
using EorDSU.Models;
using EorDSU.Repository;
using EorDSU.Repository.InterfaceRepository;
using EorDSU.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EorDSU.Common
{
    public static class BaseService
    {
        public static void AddDBService(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }
    }
}
