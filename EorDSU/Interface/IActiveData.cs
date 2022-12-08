using EorDSU.DBService;
using EorDSU.Models;

namespace EorDSU.Interface
{
    public interface IActiveData
    {
        public IQueryable<CaseCEdukind> GetCaseCEdukind();
        public IQueryable<CaseSDepartment> GetCaseSDepartments();
        public IQueryable<PersDivision> GetPersDivisions();
        public IQueryable<PersDepartment> GetPersDepartments();
        public IQueryable<Discipline> GetDisciplines();
        public IQueryable<Profile> GetProfiles();
        public IQueryable<StatusDiscipline> GetStatusDisciplines();
        public IQueryable<LevelEdu> GetLevelEdues();
        public IQueryable<FileRPD> GetFileRPDs();
        public IQueryable<FileModel> GetFileModels();
    }
}
