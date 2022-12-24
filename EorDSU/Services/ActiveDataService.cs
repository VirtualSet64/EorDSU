using BasePersonDBService.DataContext;
using DSUContextDBService.DataContext;
using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;
using Microsoft.EntityFrameworkCore;
using Sentry;
using System.IO;

namespace EorDSU.Service
{
    public class ActiveDataService : IApplicationActiveData
    {
        private readonly BASEPERSONMDFContext _bASEPERSONMDFContext;
        private readonly ApplicationContext _applicationContext;
        private readonly DSUContext _dSUContext;
        public ActiveDataService(BASEPERSONMDFContext bASEPERSONMDFContext, ApplicationContext applicationContext, DSUContext dSUContext)
        {
            _bASEPERSONMDFContext = bASEPERSONMDFContext;
            _applicationContext = applicationContext;
            _dSUContext = dSUContext;
        }

        public IQueryable<Discipline> GetDisciplines()
        {
            return _applicationContext.Disciplines
                .Include(x => x.StatusDiscipline)
                .Include(x => x.FileRPD)
                .Where(x => x.IsDeleted == false);
        }

        public IQueryable<Profile> GetProfiles()
        {
            return _applicationContext.Profiles
                .Include(x => x.FileModels)
                .Include(x => x.LevelEdu)
                .Where(x => x.IsDeleted == false);
        }

        public IQueryable<StatusDiscipline> GetStatusDisciplines()
        {
            return _applicationContext.StatusDisciplines.Where(x => x.IsDeleted == false);
        }

        public IQueryable<LevelEdu> GetLevelEdues()
        {
            return _applicationContext.LevelEdues.Where(x => x.IsDeleted == false);
        }

        public IQueryable<FileRPD> GetFileRPDs()
        {
            return _applicationContext.FileRPDs.Where(x => x.IsDeleted == false);
        }

        public IQueryable<FileModel> GetFileModels()
        {
            return _applicationContext.FileModels.Include(x => x.Profile).Where(x => x.IsDeleted == false);
        }
    }
}
