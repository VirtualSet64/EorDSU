﻿using BasePersonDBService.Interfaces;
using BasePersonDBService.Services;
using DSUContextDBService.Interfaces;
using DSUContextDBService.Services;
using Infrastructure.Repository;
using Infrastructure.Repository.InterfaceRepository;
using Infrastructure.Service;
using Infrastructure.Services.Interface;
using InfrastructureSvedenOop.Repository;
using Microsoft.Extensions.Hosting.Internal;
using SvedenOop.Services;
using SvedenOop.Services.Interfaces;

namespace SvedenOop.Common
{
    public static class BaseService
    {
        public static void AddDBService(this IServiceCollection services)
        {
            services.AddScoped<IDSUActiveData, DSUActiveData>();
            services.AddScoped<IBasePersonActiveData, BasePersonActiveData>();

            services.AddSingleton<IHostEnvironment>(new HostingEnvironment());

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
            services.AddScoped<IProfileKafedrasRepository, ProfileKafedrasRepository>();
            #endregion

            services.AddScoped<IGenerateJsonService, GenerateJsonService>();
            
        }
    }
}
