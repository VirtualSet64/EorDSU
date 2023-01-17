using EorDSU.Common.Interfaces;

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
