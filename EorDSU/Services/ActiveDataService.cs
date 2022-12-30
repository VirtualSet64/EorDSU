using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;
using Microsoft.EntityFrameworkCore;

namespace EorDSU.Service
{
    public class ActiveDataService : IApplicationActiveData
    {
        private readonly ApplicationContext _applicationContext;
        public ActiveDataService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public IQueryable<Discipline> GetDisciplines()
        {
            return _applicationContext.Disciplines
                .Include(x => x.StatusDiscipline)
                .Include(x => x.FileRPD)
                .Where(x => x.IsDeleted == false);
        }

        public async Task<Discipline> GetDisciplineById(int id)
        {
            return await _applicationContext.Disciplines
                .Include(x => x.StatusDiscipline)
                .Include(x => x.FileRPD)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<Profile> GetProfiles()
        {
            return _applicationContext.Profiles
                .Include(x => x.FileModels)
                .Include(x => x.LevelEdu)
                .Where(x => x.IsDeleted == false);
        }

        public async Task<Profile> GetProfileById(int id)
        {
            return await _applicationContext.Profiles
                .Include(x => x.FileModels)
                .Include(x => x.LevelEdu)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<StatusDiscipline> GetStatusDisciplines()
        {
            return _applicationContext.StatusDisciplines.Where(x => x.IsDeleted == false);
        }

        public Task<StatusDiscipline> GetStatusDisciplineById(int id)
        {
            return _applicationContext.StatusDisciplines.FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<LevelEdu> GetLevelEdues()
        {
            return _applicationContext.LevelEdues.Where(x => x.IsDeleted == false);
        }

        public Task<LevelEdu> GetLevelEduById(int id)
        {
            return _applicationContext.LevelEdues.FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<FileRPD> GetFileRPDs()
        {
            return _applicationContext.FileRPDs.Where(x => x.IsDeleted == false);
        }

        public Task<FileRPD> GetFileRPDById(int id)
        {
            return _applicationContext.FileRPDs.FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<FileModel> GetFileModels()
        {
            return _applicationContext.FileModels.Include(x => x.Profile).Where(x => x.IsDeleted == false);
        }

        public Task<FileModel> GetFileModelById(int id)
        {
            return _applicationContext.FileModels
                .Include(x => x.Profile)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
