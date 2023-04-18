using DSUContextDBService.DataContext;
using DSUContextDBService.Interfaces;
using DSUContextDBService.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DSUContextDBService.Services
{
    public class DSUActiveData : IDSUActiveData
    {
        private readonly DSUContext _dSUContext;
        /// <summary>
        /// 24 - Экономический колледж, 
        /// 25 - Аспирантура, 
        /// 97 - Факультет советской торговли, 
        /// 98 - Международного образования,
        /// 99 - Межфакультет
        /// </summary>
        private readonly int[] idBanFaculties = { 24, 25, 97, 98, 99 };

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

        public IQueryable<CaseSDepartment> GetCaseSDepartments()
        {
            var departments = _dSUContext.CaseSDepartments.Where(x => x.Deleted == false);

            departments = departments.Select(c => new CaseSDepartment()
            {
                DepartmentId = c.DepartmentId,
                DeptName = c.DeptName.Split('(', StringSplitOptions.None)[0],
                Abr = c.Abr,
                Code = c.Code,
                FacId = c.FacId,
                Qualification = c.Qualification,
                Deleted = c.Deleted
            });
            return departments;
        }

        public IQueryable<CaseCFaculty> GetFaculties()
        {
            return _dSUContext.CaseCFaculties.Where(x => x.Deleted == false && x.FacId > 0 && !idBanFaculties.Any(c => c == x.FacId));
        }
    }
}
