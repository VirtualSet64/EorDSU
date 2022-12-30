using EorDSU.Models;

namespace EorDSU.Interface
{
    public interface IApplicationActiveData
    {
        public IQueryable<Discipline> GetDisciplines();
        public Task<Discipline> GetDisciplineById(int id);
        public IQueryable<Profile> GetProfiles();
        public Task<Profile> GetProfileById(int id);
        public IQueryable<StatusDiscipline> GetStatusDisciplines();
        public Task<StatusDiscipline> GetStatusDisciplineById(int id);
        public IQueryable<LevelEdu> GetLevelEdues();
        public Task<LevelEdu> GetLevelEduById(int id);
        public IQueryable<FileRPD> GetFileRPDs();
        public Task<FileRPD> GetFileRPDById(int id);
        public IQueryable<FileModel> GetFileModels();
        public Task<FileModel> GetFileModelById(int id);
    }
}
