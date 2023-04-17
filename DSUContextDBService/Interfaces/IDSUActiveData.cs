using DSUContextDBService.Models;

namespace DSUContextDBService.Interfaces
{
    public interface IDSUActiveData
    {
        public Task<IQueryable<CaseSDepartment>> GetCaseSDepartments();
        public CaseSDepartment GetCaseSDepartmentById(int? id);
        public IQueryable<CaseSDepartment> GetCaseSDepartmentByFacultyId(int? id);
        public IQueryable<CaseCEdukind> GetCaseCEdukinds();
        public CaseCEdukind GetCaseCEdukindById(int? id);
        public IQueryable<CaseCFaculty> GetFaculties();
    }
}
