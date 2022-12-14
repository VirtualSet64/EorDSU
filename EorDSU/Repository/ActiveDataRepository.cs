using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;
using Microsoft.EntityFrameworkCore;
using Sentry;
using System.IO;

namespace EorDSU.Repository
{
    public class ActiveDataRepository : IActiveData
    {
        private readonly BASEPERSONMDFContext _bASEPERSONMDFContext;
        private readonly ApplicationContext _applicationContext;
        private readonly DSUContext _dSUContext;
        public ActiveDataRepository(BASEPERSONMDFContext bASEPERSONMDFContext, ApplicationContext applicationContext, DSUContext dSUContext)
        {
            _bASEPERSONMDFContext = bASEPERSONMDFContext;
            _applicationContext = applicationContext;
            _dSUContext = dSUContext;
        }

        public IQueryable<CaseSDepartment> GetCaseSDepartments()
        {
            return _dSUContext.CaseSDepartments.Where(x => x.Deleted == false);
        }

        public IQueryable<CaseCEdukind> GetCaseCEdukind()
        {
            return _dSUContext.CaseCEdukinds;
        }

        public IQueryable<PersDivision> GetPersDivisions()
        {
            return _bASEPERSONMDFContext.PersDivisions.Where(c => c.IsActive == 1 && c.IsFaculty == 1);
        }

        public IQueryable<PersDepartment> GetPersDepartments()
        {
            return _bASEPERSONMDFContext.PersDepartments.Where(c => c.IsActive == 1 && c.IsKaf == 1);
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
