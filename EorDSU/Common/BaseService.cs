using BasePersonDBService.Interfaces;
using BasePersonDBService.Services;
using DSUContextDBService.Interfaces;
using DSUContextDBService.Services;
using EorDSU.Common.Interfaces;
using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Repository;
using EorDSU.Repository.InterfaceRepository;
using EorDSU.Service;
using EorDSU.Services.Interface;

namespace EorDSU.Common
{
    public static class BaseService
    {
        public static void AddDBService(this IServiceCollection services)
        {
            services.AddScoped<IDSUActiveData, DSUActiveData>();
            services.AddScoped<IBasePersonActiveData, BasePersonActiveData>();
            services.AddScoped<IExcelParsingService, ExcelParsingService>();
            services.AddScoped<ISearchEntityService, SearchEntityService>();

            #region Repositories
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IDisciplineRepository, DisciplineRepository>();
            services.AddScoped<IFileModelRepository, FileModelRepository>();
            services.AddScoped<IFileRPDRepository, FileRPDRepository>();
            services.AddScoped<IFileTypeRepository, FileTypeRepository>();
            services.AddScoped<IStatusDisciplineRepository, StatusDisciplineRepository>();
            services.AddScoped<ILevelEduRepository, LevelEduRepository>();
            services.AddScoped<IUmuAndFacultyRepository, UmuAndFacultyRepository>();
            #endregion
        }
    }
}
