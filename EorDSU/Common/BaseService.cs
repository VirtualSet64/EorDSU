using BasePersonDBService.Interfaces;
using BasePersonDBService.Services;
using DSUContextDBService.Interfaces;
using DSUContextDBService.Services;
using Ifrastructure.Repository;
using Ifrastructure.Repository.InterfaceRepository;
using Ifrastructure.Interface;
using Ifrastructure.Service;
using Ifrastructure.Services.Interface;
using IfrastructureEorDSU.Repository;
using EorDSU.Services.Interfaces;
using EorDSU.Services;

namespace Ifrastructure.Common
{
    public static class BaseService
    {
        public static void AddDBService(this IServiceCollection services)
        {
            services.AddScoped<IDSUActiveData, DSUActiveData>();
            services.AddScoped<IBasePersonActiveData, BasePersonActiveData>();

            services.AddScoped<IExcelParsingService, ExcelParsingService>();
            services.AddScoped<ISearchEntityService, SearchEntityService>();
            services.AddScoped<IAddFileOnServer, AddFileOnServer>();

            #region Repositories
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IFileModelRepository, FileModelRepository>();
            services.AddScoped<IFileRPDRepository, FileRPDRepository>();
            services.AddScoped<IFileTypeRepository, FileTypeRepository>();
            services.AddScoped<IDisciplineRepository, DisciplineRepository>();            
            services.AddScoped<IStatusDisciplineRepository, StatusDisciplineRepository>();
            services.AddScoped<ILevelEduRepository, LevelEduRepository>();
            services.AddScoped<IUmuAndFacultyRepository, UmuAndFacultyRepository>();
            #endregion
        }
    }
}
