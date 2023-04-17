using DSUContextDBService.DataContext;
using DSUContextDBService.Interfaces;
using DSUContextDBService.Models;
using Microsoft.EntityFrameworkCore;

namespace DSUContextDBService.Services
{
    public class DSUActiveData : IDSUActiveData
    {
        private readonly DSUContext _dSUContext;

        public DSUActiveData(DSUContext dSUContext)
        {
            _dSUContext = dSUContext;
        }

        public CaseCEdukind GetCaseCEdukindById(int? id)
        {
            return _dSUContext.CaseCEdukinds.FirstOrDefault(x => x.EdukindId == id);
        }

        public IQueryable<CaseCEdukind> GetCaseCEdukinds()
        {
            return _dSUContext.CaseCEdukinds;
        }

        public CaseSDepartment GetCaseSDepartmentById(int? id)
        {
            var departments = _dSUContext.CaseSDepartments.Where(x => x.DepartmentId == id);
            return departments.Count() > 1 ? departments.FirstOrDefault(x => x.Deleted) : departments.FirstOrDefault(x => x.Deleted == false);
        }

        public IQueryable<CaseSDepartment> GetCaseSDepartmentByFacultyId(int? id)
        {
            return _dSUContext.CaseSDepartments.Where(x => x.Deleted == false && x.FacId == id);
        }

        public async Task<IQueryable<CaseSDepartment>> GetCaseSDepartments()
        {
            var departments = _dSUContext.CaseSDepartments.Where(x => x.Deleted == false);
            await departments.ForEachAsync(c => c.DeptName = c.DeptName.Split("(")[0]);
            return departments;
        }

        public IQueryable<CaseCFaculty> GetFaculties()
        {
            return _dSUContext.CaseCFaculties.Where(x => x.Deleted == false && x.FacId > 0);
        }
    }
}
