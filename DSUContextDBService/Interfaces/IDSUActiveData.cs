using DSUContextDBService.Models;

namespace DSUContextDBService.Interfaces
{
    public interface IDSUActiveData
    {
        public IQueryable<CaseSDepartment> GetCaseSDepartments();
        public CaseSDepartment GetCaseSDepartmentById(int id);
        public IQueryable<CaseSDepartment> GetCaseSDepartmentByFacultyId(int? id);
        public IQueryable<CaseCEdue> GetCaseCEdues();
        public CaseCEdue GetCaseCEdueById(int id);
        public IQueryable<CaseCEdukind> GetCaseCEdukinds();
        public CaseCEdukind GetCaseCEdukindById(int id);
    }
}
