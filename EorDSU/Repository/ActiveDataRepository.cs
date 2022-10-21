using EorDSU.DBService;
using EorDSU.Interface;
using EorDSU.Models;

namespace EorDSU.Repository
{
    public class ActiveDataRepository : IActiveData
    {
        private readonly BASEPERSONMDFContext _context;
        public ActiveDataRepository(BASEPERSONMDFContext context)
        {
            _context = context;
        }

        public List<PersDivision> GetPersDivisions()
        {
            return _context.PersDivisions.Where(c => c.IsActive == 1 && c.IsFaculty == 1).ToList();
        }

        public List<PersDepartment> GetPersDepartments()
        {
            return _context.PersDepartments.Where(c => c.IsActive == 1 && c.IsKaf == 1).ToList();
        }
    }
}
