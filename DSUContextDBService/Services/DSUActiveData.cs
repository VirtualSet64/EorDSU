using DSUContextDBService.DataContext;
using DSUContextDBService.Interfaces;
using DSUContextDBService.Models;

namespace DSUContextDBService.Services
{
    public class DSUActiveData : IDSUActiveData
    {
        private readonly DSUContext _dSUContext;

        public DSUActiveData(DSUContext dSUContext)
        {
            _dSUContext = dSUContext;
        }

        public CaseCEdukind GetCaseCEdukindById(int id)
        {
            return _dSUContext.CaseCEdukinds.FirstOrDefault(x => x.EdukindId == id);
        }

        public IQueryable<CaseCEdukind> GetCaseCEdukinds()
        {
            return _dSUContext.CaseCEdukinds;
        }

        public CaseSDepartment GetCaseSDepartmentById(int id)
        {
            return _dSUContext.CaseSDepartments.FirstOrDefault(x => x.DepartmentId == id);
        }

        public IQueryable<CaseSDepartment> GetCaseSDepartmentByFacultyId(int? id)
        {
            return _dSUContext.CaseSDepartments.Where(x => x.Deleted == false && x.FacId == id);
        }

        public IQueryable<CaseSDepartment> GetCaseSDepartments()
        {
            return _dSUContext.CaseSDepartments.Where(x => x.Deleted == false);
        }
    }
}
